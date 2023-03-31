using CMM.Core.BL.Core.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.BL.Core.Common.Environment
{
    internal static class DefaultSettings
    {
        public const string DFSettingsPath = "cmm_settings.json";

        public static UserSettingsModel DFSettings = new UserSettingsModel
        {
            Language = Languages.Russian,
            ShowProgress = true,
            ProgressBarDelay = 2000
        };
    }
}
