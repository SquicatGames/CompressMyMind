using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.DA.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.DA.Core.Services.Decompression
{
    /// <summary>
    /// Интерфейс сервиса управления распаковкой текстовых файлов
    /// </summary>
    internal interface IDecompressionService
    {
        /// <summary>
        /// Распаковать файл
        /// </summary>
        /// <param name="fileName">Имя архива (может включать путь)</param>
        /// <param name="settings">Настройки пользователя</param>
        /// <returns>True, если файл был успешно распакован</returns>
        Task<bool> TryDecompressFileAsync(
            string fileName,
            UserSettingsModel settings);
    }
}
