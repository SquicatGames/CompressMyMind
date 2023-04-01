using CMM.Core.BL.Core.Common.Localization;
using CMM.Core.BL.Core.Common.Localization.EN;
using CMM.Core.BL.Core.Common.Localization.RU;
using CMM.Core.BL.Core.Models.Settings;
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

            //replace by Activator.CreateInstance
            if (uIConstantType.Name is "UIConstantsEN")
            {
                result.Add(UIConstantsEN.Greeting);
                result.Add(UIConstantsEN.CMMDescription);
                result.Add(FillVersionString(UIConstantsEN.CMMVersion));
                result.Add(UIConstantsEN.Copyright);
            }
            else if(uIConstantType.Name is "UIConstantsRU")
            {
                result.Add(UIConstantsRU.Greeting);
                result.Add(UIConstantsRU.CMMDescription);
                result.Add(FillVersionString(UIConstantsRU.CMMVersion));
                result.Add(UIConstantsRU.Copyright);
            }

            return string.Join("\n", result);
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
