using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.DA.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.DA.Core.Services.Compression
{
    internal class CompressionService : ICompressionService
    {
        public CompressionMethods GetBestCompressionMethodByFileName(
            string fileName, 
            UserSettingsModel settings)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> TryCompressFileAsync(
            string fileName, 
            CompressionMethods method, 
            UserSettingsModel settings)
        {
            string outputFileName = $"{fileName}.cmm";

            try
            {
                using(FileStream fileStream = File.OpenRead(fileName))
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

                        //попытка сжатия
                        outputFileStream.Close();
                    }

                    fileStream.Close();
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
    }
}
