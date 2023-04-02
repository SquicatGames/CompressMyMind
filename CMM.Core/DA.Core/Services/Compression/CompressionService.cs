using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.DA.Core.Common;

namespace CMM.Core.DA.Core.Services.Compression
{
    internal class CompressionService : ICompressionService
    {
        public CompressionMethods GetBestCompressionMethodByFileName(
            string fileName, 
            UserSettingsModel settings)
        {
            byte[] buffer = new byte[CompressionConstants.BufferSize];

            int bytesRead = File.OpenRead(fileName).Read(buffer, 0, CompressionConstants.BufferSize);

            if (bytesRead == 0)
            {
                return default(CompressionMethods);
            }
            else
            {
                //Ожидаемый размер блока после сжатия различными методами
                int methodNoneResultLenght = bytesRead + 1;
                int methodRCResultLength = int.MaxValue;
                int methodHC1ResultLenght = int.MaxValue;
                int methodDC4ResultLength = int.MaxValue;

                //Расчет ожидаемого размера блока после сжатия методом RC
                {
                    int methodRCRepeatSubstringCount = 0;
                    byte previousByte = buffer[0];
                    int substringLength = 1;
                    int maxSubstringLength = 1;

                    for (int i = 1; i < bytesRead; i++)
                    {
                        if (buffer[i]==previousByte)
                        {
                            substringLength++;
                        }
                        else
                        {
                            if (substringLength > maxSubstringLength)
                                maxSubstringLength = substringLength;

                            substringLength = 1;
                            previousByte = buffer[i];
                            methodRCRepeatSubstringCount++;
                        }
                    }

                    methodRCResultLength = (int)(methodRCRepeatSubstringCount 
                        * (1 + Math.Log(maxSubstringLength, 256)));
                }

                //Расчет ожидаемого размера блока после сжатия методом HC1
                {
                    Dictionary<byte, int> hC1Map = new();
                    for (int i = 0; i < bytesRead; i++)
                    {
                        if (hC1Map.TryGetValue(
                            buffer[i],
                            out var count))
                        {
                            hC1Map[buffer[i]] = count + 1;
                        }
                        else
                        {
                            hC1Map.Add(buffer[i], 1);
                        }
                    }

                    hC1Map
                        .OrderByDescending(pair => pair.Value)
                        .ToDictionary(
                            pair => pair.Key,
                            pair => pair.Value);

                    int methodHC1MostFrequentSum = hC1Map
                        .Take(16)
                        .Sum(pair => pair.Value);

                    methodHC1ResultLenght = (int)(methodHC1MostFrequentSum * 0.5
                        + (bytesRead - methodHC1MostFrequentSum) * 1.5);
                }

                //Расчет ожидаемого размера блока после сжатия методом DC4
                {
                    Dictionary<string, int> dC4Map = new();
                    for (int i = 3; i < bytesRead; i += 4)
                    {
                        string value = string.Empty;
                        for(int p=3; p>=0; p--)
                        {
                            value += (char)buffer[i - p];
                        }    

                        if(dC4Map.TryGetValue(value, out int count))
                        {
                            dC4Map[value] = count + 1;
                        }
                        else
                        {
                            dC4Map.Add(value, 1);
                        }
                    }

                    methodDC4ResultLength = (int)(bytesRead 
                        * 0.25
                        * Math.Log(
                            dC4Map.Count(), 
                            256));
                }

                int minResultLength = Math.Min(
                    methodNoneResultLenght,
                    Math.Min(
                        methodRCResultLength,
                        Math.Min(
                            methodHC1ResultLenght,
                            methodDC4ResultLength)
                        )
                    );

                if(minResultLength == methodNoneResultLenght)
                    return CompressionMethods.None;

                if(minResultLength == methodRCResultLength)
                    return CompressionMethods.RC;

                if(minResultLength == methodHC1ResultLenght)
                    return CompressionMethods.HC1;

                if(minResultLength == methodDC4ResultLength)
                    return CompressionMethods.DC4;

                return CompressionMethods.None;
            }
        }

        public async Task<bool> TryCompressFileAsync(
            string fileName, 
            CompressionMethods method, 
            UserSettingsModel settings)
        {
            string outputFileName = $"{fileName}.cmm";

            try
            {
                using(FileStream inputFileStream = File.OpenRead(fileName))
                {
                    if(File.Exists(outputFileName))
                    {
                        File.Delete(outputFileName);
                    }

                    using (FileStream outputFileStream = File.OpenWrite(outputFileName))
                    {
                        outputFileStream.Write(new byte[1]
                        {
                            (byte)method
                        });

                        switch(method)
                        {
                            case CompressionMethods.DC4:
                                {
                                    ProcessCompressionMethodNone(
                                        inputFileStream,
                                        outputFileStream,
                                        settings);
                                } break;

                            case CompressionMethods.HC1:
                                {
                                    ProcessCompressionMethodNone(
                                        inputFileStream,
                                        outputFileStream,
                                        settings);
                                }
                                break;

                            case CompressionMethods.RC:
                                {
                                    ProcessCompressionMethodNone(
                                        inputFileStream,
                                        outputFileStream,
                                        settings);
                                }
                                break;

                            case CompressionMethods.None:
                            default:
                                {
                                    ProcessCompressionMethodNone(
                                        inputFileStream,
                                        outputFileStream,
                                        settings);

                                } break;

                        }
                        outputFileStream.Close();
                    }

                    inputFileStream.Close();
                    return true;
                }
            }
            catch(Exception ex)
            {
                //отобразить ошибку
                return false;
            }
        }

        public async Task<bool> TryCompressFileAsync(
            string fileName, 
            UserSettingsModel settings)
        {
            if(!File.Exists(fileName))
            {
                //отобразить ошибку
                return false;
            }

            try
            {
                //попытка сжатия
                return await TryCompressFileAsync(
                    fileName,
                    GetBestCompressionMethodByFileName(
                        fileName,
                        settings),
                    settings);
            }
            catch(Exception ex)
            {
                //отобразить ошибку
                return false;
            }
        }

        private void ProcessCompressionMethodNone(
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
