using CMM.Core.BL.Core.Common;
using CMM.Core.BL.Core.Common.Localization;
using CMM.Core.BL.Core.Common.Localization.EN;
using CMM.Core.BL.Core.Common.Localization.RU;
using CMM.Core.BL.Core.Models.Settings;
using EnumsNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.BL.Core.Helpers
{
    /// <summary>
    /// Вспомогательные методы для работы с пользовательским интерфейсом
    /// </summary>
    internal static class UIHelper
    { 
        /// <summary>
        /// Сформировать краткое описание программы для отображения пользователю при старте
        /// </summary>
        /// <param name="settings">Текущие настройки сотрудника (использует поле Language)</param>
        /// <returns></returns>
        public static string GetGreetingMessageBySettings(
            UserSettingsModel settings)
        {
            var result = new List<string>();

            Type uIConstantType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .First(
                    t => t.Name == 
                    $"UIConstants{LocalizationHelper.MapLanguageToAssemblySuffix(settings.Language)}"
                );

            result.Add((string)uIConstantType
                .GetField("Greeting")
                .GetRawConstantValue());

            result.Add((string)uIConstantType
                .GetField("CMMDescription")
                .GetRawConstantValue());

            result.Add(
                FillVersionString((string)uIConstantType
                .GetField("CMMVersion")
                .GetRawConstantValue())
                );

            result.Add((string)uIConstantType
                .GetField("Copyright")
                .GetRawConstantValue());

            return string.Join("\n", result);
        }

        /// <summary>
        /// Сформировать текст предупреждения на основании кода предупреждения с учетом текущих настроек пользователя
        /// </summary>
        /// <param name="code">Код предупреждения</param>
        /// <param name="settings">Текущие настройки сотрудника (использует поле Language)</param>
        /// <returns></returns>
        public static string GetWarningStringByCodeAndSettings(
            WarningCodes code,
            UserSettingsModel settings)
        {
            var warningDescription = code.AsString(EnumFormat.Description);

            Type warningCodeDescriptionType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .First(
                    t => t.Name ==
                    $"WarningCodeDescriptions{LocalizationHelper.MapLanguageToAssemblySuffix(settings.Language)}"
                );

            return (string)warningCodeDescriptionType
                .GetField(warningDescription)
                .GetRawConstantValue();
        }

        /// <summary>
        /// Сформировать строковое описание текущей версии программы с учетом локализации
        /// </summary>
        /// <param name="pattern">Локализованный паттерн строки версионирования</param>
        /// <returns></returns>
        private static string FillVersionString(string pattern)
        {
            return pattern
                .Replace(
                    "%HighVersion%",
                    AppInfo.HighVersion.ToString())
                .Replace("%LowVersion%",
                    AppInfo.LowVersion.ToString())
                .Replace("%BuildDate%",
                    AppInfo.BuildDate.ToString("dd.MM.yyyy"));
        }

    }
}
