using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.DA.Core.Common;
using CMM.Core.SL.Core.Extensions.Enum;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                using (FileStream fileStream = File.OpenRead(fileName))
                {
                    int readByteResult = fileStream.ReadByte();
                    if (readByteResult != -1)
                        header = (byte)readByteResult;

                    CompressionMethods method = (CompressionMethods)header;

                    if (TryGetCompressionMethodByHeader(
                        header,
                        out method))
                    {
                        using (FileStream outputFileStream = File.OpenWrite(outputFileName))
                        {
                            //попытка распаковки
                            outputFileStream.Close();
                            fileStream.Close();
                        }
                        return true;
                    }
                    else
                    {
                        //отобразить ошибку
                        fileStream.Close();
                        return false;
                    }
                }
            }
            catch(Exception ex) 
            {
                //отобразить ошибку
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
    }
}
