using CMM.Core.BL.Core.Common.Environment;
using CMM.Core.BL.Core.Helpers;
using CMM.Core.BL.Core.Models.Settings;
using System.Text.Json;

namespace CMM.Core.BL.Core.Services.UserSettings
{
    internal class UserSettingService : IUserSettingService
    {
        private UserSettingsModel _settings;

        private bool _isSettingFileFound;

        private bool _isSettingFileParsed;

        public UserSettingService()
        {
            LoadSettingsFromFile();
        }

        public UserSettingService(string path)
        {
            LoadSettingsFromFile(path);
        }

        public void LoadSettingsFromFile(string path = DefaultSettings.DFSettingsPath)
        {
            if (File.Exists(path))
            {
                _isSettingFileFound = true;

                try
                {
                    _settings = JsonSerializer.Deserialize<UserSettingsModel>(
                        File.ReadAllText(path))
                        ?? DefaultSettings.DFSettings;

                    _isSettingFileParsed = true;
                }
                catch
                {
                    _settings = DefaultSettings.DFSettings;
                }
            }
            else
            {
                _settings = DefaultSettings.DFSettings;
            }
        }

        public void SaveSettingsToFile()
        {
            var path = DefaultSettings.DFSettingsPath;

            try
            {
                File.WriteAllText(path, JsonSerializer.Serialize( _settings));
            }
            finally
            {
                ;//nop
            }
        }

        public UserSettingsModel GetCurrentSettings()
        {
            if(_settings == default)
            {
                return DefaultSettings.DFSettings;
            }

            return _settings;
        }

        public string GetFileSettingLoadWarnings()
        {
            if(!_isSettingFileFound
                || !_isSettingFileParsed)
            {
                return UIHelper.GetWarningStringByCodeAndSettings(
                    Common.WarningCodes.CannotLoadUserSettings,
                    _settings);
            }

            return null;
        }
    }
}
