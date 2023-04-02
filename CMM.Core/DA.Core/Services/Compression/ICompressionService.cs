using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.DA.Core.Common;

namespace CMM.Core.DA.Core.Services.Compression
{
    /// <summary>
    /// Интерфейс сервиса управления сжатием текстовых файлов
    /// </summary>
    internal interface ICompressionService
    {
        /// <summary>
        /// Выбрать оптимальный для данного файла метод сжатия
        /// </summary>
        /// <param name="fileName">Имя файла (может включать путь)</param>
        /// <param name="settings">Настройки пользователя</param>
        /// <returns></returns>
        CompressionMethods GetBestCompressionMethodByFileName(
            string fileName,
            UserSettingsModel settings);

        /// <summary>
        /// Сжать файл, используя выбранный метод сжатия
        /// </summary>
        /// <param name="fileName">Имя файла (может включать путь)</param>
        /// <param name="method">Метод сжатия</param>
        /// <param name="settings">Настройки пользователя</param>
        /// <returns>True, если файл был успешно сжат</returns>
        Task<bool> TryCompressFileAsync(
            string fileName,
            CompressionMethods method,
            UserSettingsModel settings);

        /// <summary>
        /// Сжать файл, предварительно выбрав оптимальный для него метод сжатия
        /// </summary>
        /// <param name="fileName">Имя файла (может включать путь)</param>
        /// <param name="settings">Настройки пользователя</param>
        /// <returns>True, если файл был успешно сжат</returns>
        Task<bool> TryCompressFileAsync(
            string fileName,
            UserSettingsModel settings);
    }
}
