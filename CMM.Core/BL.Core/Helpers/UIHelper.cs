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
