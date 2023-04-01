using CMM.Core.BL.Core.Common.Environment;
using CMM.Core.BL.Core.Common.Menu;
using CMM.Core.BL.Core.Common.Settings;
using CMM.Core.BL.Core.Helpers;
using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.BL.Core.Services.UserSettings;
using CMM.Core.SL.Core.Extensions.Enum;
using Microsoft.VisualBasic.FileIO;
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
                case MainMenuOptions.ChangeSettings: ProcessMainMenuChangeSettingsOption(); return;
                case MainMenuOptions.Quit: ProcessMainMenuQuitOption(); return;
                default: ProcessMainMenuQuitOption(); return;
            }
        }

        private void ProcessMainMenuChangeSettingsOption()
        {
            Console.Clear();

            //Сформировать текст сообщения при выборе пункта меню "Изменить настройки"
            //на основании настроек пользователя
            Console.WriteLine(
                MainMenuOptions.ChangeSettings
                    .GetLocalizedDescription(
                        _userSettingService.GetCurrentSettings(),
                        "MainMenuOptionDescriptions"));

            var mainMenuInputPrefix = UIHelper
                .GetMainMenuInputPrefixBySettings(
                    _userSettingService
                        .GetCurrentSettings());

            foreach (ChangeSettingsMenuOptions option in Enum.GetValues(typeof(ChangeSettingsMenuOptions)))
            {

            }

            Console.Write(mainMenuInputPrefix);

            Console.ReadLine();
        }

        private void ProcessMainMenuQuitOption()
        {
            Console.Clear();

            //Сформировать текст сообщения при выборе пункта меню "Выход"
            //на основании настроек пользователя
            Console.WriteLine(
                MainMenuOptions.Quit
                    .GetLocalizedDescription(
                        _userSettingService.GetCurrentSettings(),
                        "MainMenuOptionDescriptions"));

            _userSettingService.SaveSettingsToFile();

            Thread.Sleep(2000);
            Environment.Exit(0);
        }
    }
}
