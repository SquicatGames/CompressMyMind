using CMM.Core.BL.Core.Helpers;
using CMM.Core.BL.Core.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.SL.Core.Helpers
{
    /// <summary>
    /// Вспомогательные методы сборки
    /// </summary>
    internal static class AssemblyHelper
    {
        /// <summary>
        /// Получить представление типа класса по его названию и суффиксу локализации
        /// </summary>
        /// <param name="name">Название класса без учета суффикса локализации</param>
        /// <param name="settings">Настройки пользователя (использует свойство Language)</param>
        /// <returns></returns>
        public static Type GetTypeByNameAndSettings(
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
    }
}
