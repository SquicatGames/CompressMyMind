using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.BL.Core.Services.MainMenu;
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
        /// Проверить введенное пользователем значение на соответствие ожидаемому типу данных
        /// </summary>
        /// <param name="input">Нормализованная строка пользовательского ввода</param>
        /// <returns></returns>
        public static bool ValidateInput(string input)
        {
            Regex digitsOnly = new Regex(@"[\d]");

            return digitsOnly.IsMatch(input);
        }

        /// <summary>
        /// Уведомить пользователя об успешном сжатии данных
        /// </summary>
        /// <param name="settings">Настройки пользователя</param>
        public static void ProcessCompressionSuccessMessage(UserSettingsModel settings)
        {
            Type uIConstantType = AssemblyHelper.GetTypeByNameAndSettings(
                "UIConstants",
                settings);

            Console.WriteLine(uIConstantType
                    .GetConstString("CompressionSuccessMessage"));

            var prefix = GetMainMenuInputPrefixBySettings(settings);

            Console.Write(prefix);
            Console.ReadLine();
        }

        /// <summary>
        /// Уведомить пользователя об ошибке сжатия
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        /// <param name="settings">Настройки пользователя</param>
        public static void ProcessCompressionErrorMessage(
            string message,
            UserSettingsModel settings)
        {
            Type uIConstantType = AssemblyHelper.GetTypeByNameAndSettings(
                "UIConstants",
                settings);

            Console.WriteLine(FillErrorMessageString(
                uIConstantType
                    .GetConstString("CompressionErrorMessagePattern"),
                message));

            var prefix = GetMainMenuInputPrefixBySettings(settings);

            Console.Write(prefix);
            Console.ReadLine();
        }

        /// <summary>
        /// Уведомить пользователя об успешной распаковке данных
        /// </summary>
        /// <param name="settings">Настройки пользователя</param>
        public static void ProcessDecompressionSuccessMessage(UserSettingsModel settings)
        {
            Type uIConstantType = AssemblyHelper.GetTypeByNameAndSettings(
                "UIConstants",
                settings);

            Console.WriteLine(uIConstantType
                    .GetConstString("DecompressionSuccessMessage"));

            var prefix = GetMainMenuInputPrefixBySettings(settings);

            Console.Write(prefix);
            Console.ReadLine();
        }

        /// <summary>
        /// Уведомить пользователя об ошибке распаковки
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        /// <param name="settings">Настройки пользователя</param>
        public static void ProcessDecompressionErrorMessage(
            string message,
            UserSettingsModel settings)
        {
            Type uIConstantType = AssemblyHelper.GetTypeByNameAndSettings(
                "UIConstants",
                settings);

            Console.WriteLine(FillErrorMessageString(
                uIConstantType
                    .GetConstString("DecompressionErrorMessagePattern"),
                message));

            var prefix = GetMainMenuInputPrefixBySettings(settings);

            Console.Write(prefix);
            Console.ReadLine();
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
        /// Сформировать строковое описание ошибки с учетом локализации
        /// </summary>
        /// <param name="pattern">Локализованный паттерн строки отображения ошибки</param>
        /// <param name="message">Оригинальный текст ошибки</param>
        /// <returns></returns>
        private static string FillErrorMessageString(
            string pattern,
            string message)
        {
            return pattern
                .Replace(
                    "%Message%",
                    message);
        }
    }
}
