using CMM.Core.BL.Core.Common.Menu;
using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.SL.Core.Extensions.Enum;
using CMM.Core.SL.Core.Extensions.TypeExt;
using CMM.Core.SL.Core.Helpers;
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

            Type uIConstantType = AssemblyHelper.GetTypeByNameAndSettings(
                "UIConstants",
                settings);

            result.Add(
                uIConstantType.GetConstString("Greeting"));

            result.Add(
                uIConstantType.GetConstString("CMMDescription"));

            result.Add(
                FillVersionString(
                    uIConstantType.GetConstString("CMMVersion")
                ));

            result.Add(
                uIConstantType.GetConstString("Copyright"));

            return string.Join("\n", result);
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

            Type uIConstantType = AssemblyHelper.GetTypeByNameAndSettings(
                "MainMenuConstants",
                settings);

            result.Add(uIConstantType
                .GetConstString("MainMenuHeader"));

            result.Add(uIConstantType
                .GetConstString("MainMenuOptionCompress"));

            result.Add(uIConstantType
                .GetConstString("MainMenuOptionDecompress"));

            result.Add(uIConstantType
                .GetConstString("MainMenuOptionChangeSettings"));

            result.Add(uIConstantType
                .GetConstString("MainMenuOptionQuit"));

            return string.Join("\n", result);
        }

        /// <summary>
        /// Сформировать представление префикса ввода данных на основании настроек пользователя
        /// </summary>
        /// <param name="settings">Текущие настройки пользователя (использует свойство Language)</param>
        /// <returns></returns>
        public static string GetMainMenuInputPrefixBySettings(UserSettingsModel settings)
        {
            Type uIConstantType = AssemblyHelper.GetTypeByNameAndSettings(
                "UIConstants",
                settings);

            return uIConstantType.GetConstString("MainMenuInputPrefix");
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
