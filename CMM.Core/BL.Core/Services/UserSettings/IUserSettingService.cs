using CMM.Core.BL.Core.Common.Environment;
using CMM.Core.BL.Core.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.BL.Core.Services.UserSettings
{
    internal interface IUserSettingService
    {
        void LoadSettingsFromFile(string path = DefaultSettings.DFSettingsPath);

        UserSettingsModel GetCurrentSettings();

        string GetFileSettingLoadWarnings();
    }
}
