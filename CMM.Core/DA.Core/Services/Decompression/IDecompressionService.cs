using CMM.Core.BL.Core.Models.Settings;

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
