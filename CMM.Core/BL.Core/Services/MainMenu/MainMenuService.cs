using CMM.Core.BL.Core.Common.Environment;
using CMM.Core.BL.Core.Common.Menu;
using CMM.Core.BL.Core.Helpers;
using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.BL.Core.Services.UserSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.BL.Core.Services.MainMenu
{
    internal class MainMenuService : IMainMenuService
    {
        private IUserSettingService _userSettingService;

        public MainMenuService()
        {
            _userSettingService = new UserSettingService();
        }

        public MainMenuService(IUserSettingService settingService)
        {
            _userSettingService = settingService;
        }

        public async Task ProcessMainMenuOptionAsync(MainMenuOptions option)
        {
            switch (option)
            {
                case MainMenuOptions.Quit: ProcessMainMenuQuitOption(); return;
                default: ProcessMainMenuQuitOption(); return;
            }
        }

        private void ProcessMainMenuQuitOption()
        {
            Console.WriteLine(
                UIHelper.GetMainMenuOptionTitleBySettings(
                    MainMenuOptions.Quit,
                    _userSettingService.GetCurrentSettings())
                );

            _userSettingService.SaveSettingsToFile();

            Thread.Sleep(2000);
            Environment.Exit(0);
        }
    }
}
