using CMM.Core.BL.Core.Common;
using CMM.Core.BL.Core.Models.Settings;
using EnumsNET;
using System.Reflection;

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
        /// <param name="settings">Текущие настройки пользователя (использует свойство Language)</param>
        /// <returns></returns>
        public static string GetGreetingMessageBySettings(
            UserSettingsModel settings)
        {
            var result = new List<string>();

            Type uIConstantType = GetTypeByNameAndSettings(
                "UIConstants",
                settings);

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
        /// <param name="settings">Текущие настройки пользователя (использует свойство Language)</param>
        /// <returns></returns>
        public static string GetWarningStringByCodeAndSettings(
            WarningCodes code,
            UserSettingsModel settings)
        {
            var warningDescription = code.AsString(EnumFormat.Description);

            Type warningCodeDescriptionType = GetTypeByNameAndSettings(
                "WarningCodeDescriptions",
                settings);

            return (string)warningCodeDescriptionType
                .GetField(warningDescription)
                .GetRawConstantValue();
        }

        /// <summary>
        /// Сформировать описание основных опций программы (главное меню) на основании настроек пользователя
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string GetMainMenuStringBySettings(
            UserSettingsModel settings)
        {
            var result = new List<string>();

            Type uIConstantType = GetTypeByNameAndSettings(
                "UIConstants",
                settings);

            result.Add((string)uIConstantType
                .GetField("MainMenuHeader")
                .GetRawConstantValue());

            result.Add((string)uIConstantType
                .GetField("MainMenuOptionCompress")
                .GetRawConstantValue());

            result.Add((string)uIConstantType
                .GetField("MainMenuOptionDecompress")
                .GetRawConstantValue());

            result.Add((string)uIConstantType
                .GetField("MainMenuOptionChangeSettings")
                .GetRawConstantValue());

            result.Add((string)uIConstantType
                .GetField("MainMenuOptionQuit")
                .GetRawConstantValue());

            result.Add((string)uIConstantType
                .GetField("MainMenuInputPrefix")
                .GetRawConstantValue());

            return string.Join("\n", result);
        }

        /// <summary>
        /// Получить представление типа класса по его названию и суффиксу локализации
        /// </summary>
        /// <param name="name">Название класса без учета суффикса локализации</param>
        /// <param name="settings">Настройки пользователя (использует свойство Language)</param>
        /// <returns></returns>
        private static Type GetTypeByNameAndSettings(
            string name,
            UserSettingsModel settings)
        {
            return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .First(
                    t => t.Name ==
                    $"{name}{LocalizationHelper.MapLanguageToAssemblySuffix(settings.Language)}"
                );
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
