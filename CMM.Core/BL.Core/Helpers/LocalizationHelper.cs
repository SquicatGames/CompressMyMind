using CMM.Core.BL.Core.Common;
using CMM.Core.BL.Core.Common.Environment;

namespace CMM.Core.BL.Core.Helpers
{
    /// <summary>
    /// Вспомогательные методы для управления представлением данных с учетом языковых настроек пользователя
    /// </summary>
    internal static class LocalizationHelper
    {
        /// <summary>
        /// Получить суффикс наименования классов сборки, относящихся к выбранному языку
        /// </summary>
        /// <param name="language">Язык пользовательских настроек</param>
        /// <returns></returns>
        public static string MapLanguageToAssemblySuffix(
            Languages language) =>
            language switch
            {
                Languages.English => "EN",
                Languages.Russian => "RU",
                _ => MapLanguageToAssemblySuffix(DefaultSettings.DFSettings.Language),
            };
    }
}
