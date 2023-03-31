using CMM.Core.BL.Core.Common;
using CMM.Core.BL.Core.Common.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.BL.Core.Helpers
{
    internal static class LocalizationHelper
    {
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
