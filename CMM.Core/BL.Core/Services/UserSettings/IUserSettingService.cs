using CMM.Core.BL.Core.Common.Environment;
using CMM.Core.BL.Core.Models.Settings;

namespace CMM.Core.BL.Core.Services.UserSettings
{
    /// <summary>
    /// Интерфейс сервиса управления настроками пользователя программы
    /// </summary>
    internal interface IUserSettingService
    {
        /// <summary>
        /// Загрузить настройки пользователя из файла
        /// </summary>
        /// <param name="path">Расположение и имя файла с настройками (ожидается формат JSON)</param>
        void LoadSettingsFromFile(string path = DefaultSettings.DFSettingsPath);

        /// <summary>
        /// Сохранить настройки пользователя в файл
        /// </summary>
        void SaveSettingsToFile();

        /// <summary>
        /// Получить текущие настройки пользователя
        /// </summary>
        /// <returns>Возвращает активные на момент запуска программы настройки</returns>
        UserSettingsModel GetCurrentSettings();

        /// <summary>
        /// Сформировать строковое представление списка предупреждений, возникших при загрузке
        /// файла настроек, для отображения пользователю
        /// </summary>
        /// <returns>Строка с локализованным текстом предупреждений или NULL</returns>
        string GetFileSettingLoadWarnings();

        /// <summary>
        /// Изменить текущие настройки пользователя
        /// </summary>
        /// <param name="newSettings">Новое значение пользовательских настроек</param>
        void SetCurrentSettings(UserSettingsModel newSettings);
    }
}
