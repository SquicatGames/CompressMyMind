using CMM.Core.BL.Core.Common;
using CMM.Core.BL.Core.Common.Menu;
using CMM.Core.BL.Core.Models.Settings;
using EnumsNET;
using System.Reflection;
using System.Text.RegularExpressions;

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
        /// <param name="settings">Текущие настройки пользователя (использует свойство Language)</param>
        /// <returns></returns>
        public static string GetMainMenuStringBySettings(
            UserSettingsModel settings)
        {
            var result = new List<string>();

            Type uIConstantType = GetTypeByNameAndSettings(
                "MainMenuConstants",
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

            return string.Join("\n", result);
        }

        /// <summary>
        /// Сформировать представление префикса ввода данных на основании настроек пользователя
        /// </summary>
        /// <param name="settings">Текущие настройки пользователя (использует свойство Language)</param>
        /// <returns></returns>
        public static string GetMainMenuInputPrefixBySettings(UserSettingsModel settings)
        {
            Type uIConstantType = GetTypeByNameAndSettings(
                "UIConstants",
                settings);

            return (string)uIConstantType
                .GetField("MainMenuInputPrefix")
                .GetRawConstantValue();
        }

        /// <summary>
        /// Сформировать текст сообщения при выборе пункта меню на основании настроек пользователя
        /// </summary>
        /// <param name="option">Выбранный пункт меню</param>
        /// <param name="settings">Настройки пользователя</param>
        /// <returns></returns>
        public static string GetMainMenuOptionTitleBySettings(
            MainMenuOptions option,
            UserSettingsModel settings)
        {
            var mainMenuOptionDescription = option.AsString(EnumFormat.Description);

            Type mainMenuOptionDescriptionsType = GetTypeByNameAndSettings(
                "MainMenuOptionDescriptions",
                settings);

            return (string)mainMenuOptionDescriptionsType
                .GetField(mainMenuOptionDescription)
                .GetRawConstantValue();
        }

        /// <summary>
        /// Отпределить опцию меню по введенной пользователем строке
        /// </summary>
        /// <param name="input">Данные, введенные пользователем</param>
        /// <param name="option">Опция меню</param>
        /// <returns></returns>
        public static bool TryGetMenuOptionFromString(
            string input, 
            out MainMenuOptions option)
        {
            option = default(MainMenuOptions);

            string normalizedInput = input.Replace(" ", "");

            if(!ValidateMainMenuInput(normalizedInput))
                return false;

            if(MainMenuOptions.TryParse(
                normalizedInput, 
                out option))
            {
                if (option.IsValid())
                    return true;
            }

            return false;
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

        /// <summary>
        /// Проверить введенное пользователем значение на соответствие ожидаемому типу данных
        /// </summary>
        /// <param name="input">Нормализованная строка пользовательского ввода</param>
        /// <returns></returns>
        private static bool ValidateMainMenuInput(string input)
        {
            Regex digitsOnly = new Regex(@"[\d]");

            return digitsOnly.IsMatch(input);
        }
    }
}
