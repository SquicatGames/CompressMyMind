using CMM.Core.BL.Core.Models.Settings;

namespace CMM.Core.BL.Core.Common.Environment
{
    /// <summary>
    /// Класс настроек по умолчанию
    /// </summary>
    internal static class DefaultSettings
    {
        /// <summary>
        /// Имя и расположение файла настроек по умолчанию
        /// </summary>
        public const string DFSettingsPath = "cmm_settings.json";

        /// <summary>
        /// Представление настроек по умолчанию
        /// </summary>
        public static UserSettingsModel DFSettings = new UserSettingsModel
        {
            Language = Languages.Russian,
            ShowProgress = true,
            ProgressBarDelay = 300
        };
    }
}
