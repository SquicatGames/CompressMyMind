using CMM.Core.BL.Core.Helpers;
using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.DA.Core.Common;
using CMM.Core.DA.Core.Common.Helpers;
using CMM.Core.SL.Core.Extensions.Enum;
using System.Diagnostics;
using System.Text.Json;

namespace CMM.Core.DA.Core.Services.Decompression
{
    internal class DecompressionService : IDecompressionService
    {
        public async Task<bool> TryDecompressFileAsync(
            string fileName, 
            UserSettingsModel settings)
        {
            //Получить первый символ (заголовок) файла и определить использованный метод сжатия
            byte header = (byte)default(CompressionMethods);

            string outputFileName = fileName.EndsWith(".cmm")
                ? fileName.Substring(0, fileName.Length - 4)
                : $"{fileName}_decompressed.txt";

            try
            {
                using (FileStream inputFileStream = File.OpenRead(fileName))
                {
                    int readByteResult = inputFileStream.ReadByte();
                    if (readByteResult != -1)
                        header = (byte)readByteResult;

                    CompressionMethods method = (CompressionMethods)header;

                    if (TryGetCompressionMethodByHeader(
                        header,
                        out method))
                    {
                        using (FileStream outputFileStream = File.OpenWrite(outputFileName))
                        {
                            switch (method)
                            {
                                case CompressionMethods.DC4:
                                    {
                                        ProcessDecompressionMethodHC1(
                                        inputFileStream,
                                        outputFileStream,
                                        settings);
                                    }
                                    break;

                                case CompressionMethods.HC1:
                                    {
                                        ProcessDecompressionMethodHC1(
                                        inputFileStream,
                                        outputFileStream,
                                        settings);
                                    }
                                    break;

                                case CompressionMethods.RC:
                                    {
                                        ProcessDecompressionMethodHC1(
                                        inputFileStream,
                                        outputFileStream,
                                        settings);
                                    }
                                    break;

                                case CompressionMethods.None:
                                default:
                                    {
                                        ProcessDecompressionMethodNone(
                                        inputFileStream,
                                        outputFileStream,
                                        settings);
                                    }
                                    break;

                            }
                            outputFileStream.Close();
                            inputFileStream.Close();
                        }
                        UIHelper.ProcessDecompressionSuccessMessage(settings);
                        return true;
                    }
                    else
                    {
                        UIHelper.ProcessDecompressionErrorMessage(
                            "Unknown file format",
                            settings);

                        inputFileStream.Close();
                        return false;
                    }
                }
            }
            catch(Exception ex) 
            {
                UIHelper.ProcessDecompressionErrorMessage(
                            ex.Message,
                            settings);
                return false;
            }
        }

        private bool TryGetCompressionMethodByHeader(
            byte header,
            out CompressionMethods method)
        {
            method = default(CompressionMethods);

            if(CompressionMethods.TryParse(
                header.ToString(),
                out method))
            {
                if (method.IsValid())
                    return true;
            }

            return false;
        }

        private void ProcessDecompressionMethodNone(
            FileStream inStream,
            FileStream outStream,
            UserSettingsModel settings)
        {
            int bytesRead = 1;
            long bytesProcessed = 1;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            while (bytesRead > 0)
            {
                byte[] bytes = new byte[CompressionConstants.BufferSize];
                bytesRead = inStream.Read(
                    bytes,
                    0,
                    bytes.Length);

                outStream.Write(bytes, 0, bytesRead);

                bytesProcessed += bytesRead;

                if (settings.ShowProgress)
                {
                    if (sw.ElapsedMilliseconds >= settings.ProgressBarDelay)
                    {
                        UIHelper.ProcessCurrentFileProcessingStateMessage(
                            bytesProcessed,
                            settings);

                        sw.Reset();
                        sw.Start();
                    }
                }
            }
        }

        private void ProcessDecompressionMethodHC1(
            FileStream inStream,
            FileStream outStream,
            UserSettingsModel settings)
        {
            int bytesRead = 1;
            long bytesProcessed = 1;

            Dictionary<byte, CompressionAtom> charsetInfo = new();
            Dictionary<CompressionAtom, byte> decompressionInfo = new();

            byte cILengthLowPart = (byte)inStream.ReadByte();
            byte cILengthHighPart = (byte)inStream.ReadByte();

            int cILength = cILengthLowPart +
                cILengthHighPart * 256;

            byte[] rawCharsetInfo = new byte[cILength];

            bytesRead = inStream
                .Read(
                    rawCharsetInfo, 
                    0, 
                    cILength);

            bytesProcessed += bytesRead;

            if(bytesRead == cILength)
            {
                charsetInfo = JsonSerializer
                    .Deserialize<Dictionary<byte, CompressionAtom>>(
                        new string(rawCharsetInfo
                            .Select(b => (char)b)
                            .ToArray())) 
                    ?? new Dictionary<byte, CompressionAtom>();

                int index = 0;

                Dictionary<byte, CompressionAtom> restoredCharsetInfo = new();
                foreach(var pair in charsetInfo)
                {
                    if (index < 15)
                    {
                        restoredCharsetInfo.Add(
                            pair.Key,
                            new CompressionAtom
                            {
                                Code = index,
                                CodeLen = 4
                            });
                    }
                    else
                    {
                        restoredCharsetInfo.Add(
                            pair.Key,
                            new CompressionAtom
                            {
                                Code = (15 << 8) + index,
                                CodeLen = 12
                            });
                    }
                    index++;
                }

                decompressionInfo = CompressionAtomHelper
                    .ReflectByteMap(restoredCharsetInfo);

                //process file data
                Stopwatch sw = new Stopwatch();
                sw.Start();

                while (bytesRead != 0)
                {
                    byte bytesToProcessLowPart = (byte)inStream.ReadByte();
                    byte bytesToProcessHighPart = (byte)inStream.ReadByte();

                    int bytesToProcess = bytesToProcessLowPart
                        + bytesToProcessHighPart * 256;

                    bytesToProcess -= 2;

                    byte[] buffer = new byte[bytesToProcess];

                    bytesRead = inStream.Read(
                        buffer,
                        0,
                        bytesToProcess);

                    bytesProcessed += bytesRead;

                    if(bytesRead != bytesToProcess)
                    {
                        break;
                    }

                    var atoms = CompressionAtomHelper
                        .GetCompressionAtomsByBytes(buffer);

                    foreach(var atom in atoms)
                    {
                        outStream.WriteByte(decompressionInfo[atom]);
                    }

                    if (settings.ShowProgress)
                    {
                        if (sw.ElapsedMilliseconds >= settings.ProgressBarDelay)
                        {
                            UIHelper.ProcessCurrentFileProcessingStateMessage(
                                bytesProcessed,
                                settings);

                            sw.Reset();
                            sw.Start();
                        }
                    }
                }
            }
        }
    }
}
