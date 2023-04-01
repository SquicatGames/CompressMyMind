using CMM.Core.BL.Core.Common.Menu;
using CMM.Core.BL.Core.Common.Settings;
using CMM.Core.BL.Core.Helpers;
using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.BL.Core.Services.UserSettings;
using CMM.Core.SL.Core.Extensions.Enum;
using CMM.Core.SL.Core.Extensions.TypeExt;
using CMM.Core.SL.Core.Helpers;
using System.Reflection;

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

        public async Task ShowMainMenuAsync(bool cleanConsole = true)
        {
            if (cleanConsole)
                Console.Clear();

            var mainMenuData = GetMainMenuStringBySettings(_userSettingService.GetCurrentSettings());
            var mainMenuInputPrefix = GetMainMenuInputPrefixBySettings(_userSettingService.GetCurrentSettings());

            //Отображение главного меню программы
            Console.WriteLine(mainMenuData);

            bool anyMainMenuOptionSelected = false;
            MainMenuOptions selectedOption = default(MainMenuOptions);

            while (!anyMainMenuOptionSelected)
            {
                Console.Write(mainMenuInputPrefix);
                if (TryGetMenuOptionFromString(
                    Console.ReadLine(),
                    out MainMenuOptions option))
                {
                    anyMainMenuOptionSelected = true;
                    selectedOption = option;
                }
            }

            //Выполнить действия в соответствие с выбором пользователя
            await ProcessMainMenuOptionAsync(selectedOption);
        }

        /// <summary>
        /// Сформировать описание основных опций программы (главное меню) на основании настроек пользователя
        /// </summary>
        /// <param name="settings">Текущие настройки пользователя (использует свойство Language)</param>
        /// <returns></returns>
        private static string GetMainMenuStringBySettings(
            UserSettingsModel settings)
        {
            var result = new List<string>();

            Type uIConstantType = AssemblyHelper.GetTypeByNameAndSettings(
                "MainMenuConstants",
                settings);

            result.Add(uIConstantType
                .GetConstString("MainMenuHeader"));

            result.Add(uIConstantType
                .GetConstString("MainMenuOptionCompress"));

            result.Add(uIConstantType
                .GetConstString("MainMenuOptionDecompress"));

            result.Add(uIConstantType
                .GetConstString("MainMenuOptionChangeSettings"));

            result.Add(uIConstantType
                .GetConstString("MainMenuOptionQuit"));

            return string.Join("\n", result);
        }

        /// <summary>
        /// Сформировать представление префикса ввода данных на основании настроек пользователя
        /// </summary>
        /// <param name="settings">Текущие настройки пользователя (использует свойство Language)</param>
        /// <returns></returns>
        private static string GetMainMenuInputPrefixBySettings(UserSettingsModel settings)
        {
            Type uIConstantType = AssemblyHelper.GetTypeByNameAndSettings(
                "UIConstants",
                settings);

            return uIConstantType.GetConstString("MainMenuInputPrefix");
        }

        /// <summary>
        /// Отпределить опцию меню по введенной пользователем строке
        /// </summary>
        /// <param name="input">Данные, введенные пользователем</param>
        /// <param name="option">Опция меню</param>
        /// <returns></returns>
        private static bool TryGetMenuOptionFromString(
            string input,
            out MainMenuOptions option)
        {
            option = default(MainMenuOptions);

            string normalizedInput = input.Replace(" ", "");

            if (!UIHelper.ValidateInput(normalizedInput))
                return false;

            if (MainMenuOptions.TryParse(
                normalizedInput,
                out option))
            {
                if (option.IsValid())
                    return true;
            }

            return false;
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

            var mainMenuInputPrefix = GetMainMenuInputPrefixBySettings(
                _userSettingService
                    .GetCurrentSettings());

            foreach (ChangeSettingsMenuOptions option in Enum.GetValues(typeof(ChangeSettingsMenuOptions)))
            {
                var description = option.GetLocalizedDescription(
                    _userSettingService
                        .GetCurrentSettings(),
                    "UserSettingPropertyDescriptions");

                Console.WriteLine($"({(int)option}) {description}");
            }

            Console.WriteLine(AssemblyHelper
                .GetTypeByNameAndSettings(
                    "MainMenuConstants",
                    _userSettingService
                        .GetCurrentSettings())
                .GetConstString("BackToMainMenuGlobalOption"));

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
