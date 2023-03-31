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
    internal static class UIHelper
    { 
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
            }
            else if(uIConstantType.Name is "UIConstantsRU")
            {
                result.Add(UIConstantsRU.Greeting);
                result.Add(UIConstantsRU.CMMDescription);
            }

            return string.Join("\n", result);
        }
    }
}
