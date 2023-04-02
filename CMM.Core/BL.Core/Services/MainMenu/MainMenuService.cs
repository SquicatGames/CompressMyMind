using CMM.Core.BL.Core.Common.Localization.RU;
using CMM.Core.BL.Core.Common.Menu;
using CMM.Core.BL.Core.Common.Settings;
using CMM.Core.BL.Core.Helpers;
using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.BL.Core.Services.UserSettings;
using CMM.Core.SL.Core.Extensions.Enum;
using CMM.Core.SL.Core.Extensions.TypeExt;
using CMM.Core.SL.Core.Helpers;
using System.Linq.Expressions;
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

        public async Task ProcessMainMenuOptionAsync(MainMenuOptions option)
        {
            switch (option)
            {
                case MainMenuOptions.Compress: await ProcessMainMenuCompressOptionAsync(); return;
                case MainMenuOptions.Decompress: await ProcessMainMenuDecompressOptionAsync(); return;
                case MainMenuOptions.ChangeSettings: await ProcessMainMenuChangeSettingsOption(); return;
                case MainMenuOptions.Quit: ProcessMainMenuQuitOption(); return;
                default: ProcessMainMenuQuitOption(); return;
            }
        }

        public async Task ProcessChangeSettingsMenuOptionAsync(ChangeSettingsMenuOptions option)
        {
            switch (option)
            {
                case ChangeSettingsMenuOptions.Language: ProcessChangeSettingsMenuLanguageOption(); return;
                case ChangeSettingsMenuOptions.ShowProgress: ProcessChangeSettingsMenuShowProgressOption(); return;
                case ChangeSettingsMenuOptions.ProgressBarDelay: await ProcessChangeSettingsMenuProgressBarDelayOption(); return;
                default: await ShowMainMenuAsync(); return;
            }
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

        private async Task ProcessMainMenuCompressOptionAsync()
        {

        }

        private async Task ProcessMainMenuDecompressOptionAsync()
        {

        }

        private async Task ProcessMainMenuChangeSettingsOption()
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

            bool anyChangeSettingsMenuOptionSelected = false;
            ChangeSettingsMenuOptions selectedOption = default(ChangeSettingsMenuOptions);

            //Получить корректный ввод от пользователя программы
            while (!anyChangeSettingsMenuOptionSelected)
            {
                Console.Write(mainMenuInputPrefix);
                if (TryGetChangeSettingsMenuOptionFromString(
                    Console.ReadLine(),
                    out ChangeSettingsMenuOptions option))
                {
                    anyChangeSettingsMenuOptionSelected = true;
                    selectedOption = option;
                }
            }

            //Выполнить действия в соответствие с выбором пользователя
            if ((int)selectedOption == 0)
            {
                ShowMainMenuAsync();
            }
            else
            {
                await ProcessChangeSettingsMenuOptionAsync(selectedOption);
            }
        }

        private static bool TryGetChangeSettingsMenuOptionFromString(
            string input,
            out ChangeSettingsMenuOptions option)
        {
            option = default(ChangeSettingsMenuOptions);

            string normalizedInput = input.Replace(" ", "");

            if (!UIHelper.ValidateInput(normalizedInput))
                return false;

            if (ChangeSettingsMenuOptions.TryParse(
                normalizedInput,
                out option))
            {
                if ((int)option == 0)
                    return true;

                if (option.IsValid())
                    return true;
            }

            return false;
        }

        private void ProcessChangeSettingsMenuLanguageOption()
        {

        }

        private void ProcessChangeSettingsMenuShowProgressOption()
        {

        }

        private async Task ProcessChangeSettingsMenuProgressBarDelayOption()
        {
            Console.Clear();

            var currentSettings = _userSettingService.GetCurrentSettings();

            Console.WriteLine(
                ChangeSettingsMenuOptions.ProgressBarDelay
                    .GetLocalizedDescription(
                        currentSettings,
                        "UserSettingPropertyDescriptions"));

            var changeSettingsMenuConstantsType = AssemblyHelper
                .GetTypeByNameAndSettings(
                    "ChangeSettingsMenuConstants",
                    currentSettings);

            Console.WriteLine(changeSettingsMenuConstantsType
                .GetConstString("ChangeSettingsProgressBarDelayOptionTitle"));

            var currentValuePrefix = changeSettingsMenuConstantsType
                .GetConstString("ChangeSettingsShowCurrentValueMessage");

            Console.WriteLine($"{currentValuePrefix}{currentSettings.ProgressBarDelay}");

            var mainMenuInputPrefix = GetMainMenuInputPrefixBySettings(
                _userSettingService
                    .GetCurrentSettings());

            bool anyValidDelayValueTyped = false;
            int newDelayValue = 0;

            //Получить корректный ввод от пользователя программы
            while (!anyValidDelayValueTyped)
            {
                Console.Write(mainMenuInputPrefix);
                if (TryGetProgressBarDelayValueFromString(
                    Console.ReadLine(),
                    out int delayValue))
                {
                    anyValidDelayValueTyped = true;
                    newDelayValue = delayValue;
                }
            }

            if (newDelayValue == 0)
                await ProcessMainMenuChangeSettingsOption();
            else
            {
                currentSettings.ProgressBarDelay = newDelayValue;

                //Сохранение новых настроек
                _userSettingService.SetCurrentSettings(currentSettings);

                Console.WriteLine(changeSettingsMenuConstantsType
                    .GetConstString("ChangeSettingsSuccessMessage"));

                Console.Write(mainMenuInputPrefix);
                Console.ReadLine();

                await ProcessMainMenuChangeSettingsOption();
            }
        }

        private bool TryGetProgressBarDelayValueFromString(
            string input, 
            out int delayValue)
        {
            delayValue = 0;

            string normalizedInput = input.Replace(" ", "");

            if (!UIHelper.ValidateInput(normalizedInput))
                return false;

            if(int.TryParse(
                normalizedInput,
                out int value))
            {
                delayValue = value;
                return true;
            }

            return false;
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
