using CMM.Core.BL.Core.Helpers;
using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.DA.Core.Common;
using CMM.Core.SL.Core.Extensions.Enum;

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
                                        ProcessDecompressionMethodNone(
                                        inputFileStream,
                                        outputFileStream,
                                        settings);
                                    }
                                    break;

                                case CompressionMethods.HC1:
                                    {
                                        ProcessDecompressionMethodNone(
                                        inputFileStream,
                                        outputFileStream,
                                        settings);
                                    }
                                    break;

                                case CompressionMethods.RC:
                                    {
                                        ProcessDecompressionMethodNone(
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
            while (bytesRead > 0)
            {
                byte[] bytes = new byte[CompressionConstants.BufferSize];
                bytesRead = inStream.Read(
                    bytes,
                    0,
                    bytes.Length);

                outStream.Write(bytes, 0, bytesRead);
            }
        }
    }
}
