using CMM.Core.BL.Core.Common;
using CMM.Core.BL.Core.Common.Localization.EN;
using CMM.Core.BL.Core.Common.Localization.RU;
using CMM.Core.BL.Core.Common.Menu;
using CMM.Core.BL.Core.Common.Settings;
using CMM.Core.BL.Core.Helpers;
using CMM.Core.BL.Core.Models.Settings;
using CMM.Core.BL.Core.Services.UserSettings;
using CMM.Core.SL.Core.Extensions.BoolExt;
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
                case MainMenuOptions.ChangeSettings: await ProcessMainMenuChangeSettingsOptionAsync(); return;
                case MainMenuOptions.Quit: ProcessMainMenuQuitOption(); return;
                default: ProcessMainMenuQuitOption(); return;
            }
        }

        public async Task ProcessChangeSettingsMenuOptionAsync(ChangeSettingsMenuOptions option)
        {
            switch (option)
            {
                case ChangeSettingsMenuOptions.Language: await ProcessChangeSettingsMenuLanguageOptionAsync(); return;
                case ChangeSettingsMenuOptions.ShowProgress: await ProcessChangeSettingsMenuShowProgressOptionAsync(); return;
                case ChangeSettingsMenuOptions.ProgressBarDelay: await ProcessChangeSettingsMenuProgressBarDelayOptionAsync(); return;
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
            Console.Clear();

            //Сформировать текст сообщения при выборе пункта меню "Указать файл для сжатия"
            //на основании настроек пользователя
            Console.WriteLine(
                MainMenuOptions.Compress
                    .GetLocalizedDescription(
                        _userSettingService.GetCurrentSettings(),
                        "MainMenuOptionDescriptions"));

            var mainMenuInputPrefix = GetMainMenuInputPrefixBySettings(
                _userSettingService
                    .GetCurrentSettings());

            var compressMenuConstantsType = AssemblyHelper
                .GetTypeByNameAndSettings(
                    "CompressMenuConstants",
                    _userSettingService.GetCurrentSettings());

            var fileNotFoundMessage = compressMenuConstantsType
                .GetConstString("FileNotFoundMessage");

            bool anyValidFilePathTyped = false;
            string sourceFileName = string.Empty;

            while(!anyValidFilePathTyped)
            {
                Console.Write(mainMenuInputPrefix);
                if(TryGetFileByPath(
                    Console.ReadLine(),
                    out string fileName))
                {
                    anyValidFilePathTyped = true;
                    sourceFileName =fileName;
                }
                else
                {
                    Console.WriteLine(fileNotFoundMessage);
                }
            }

            if(sourceFileName == null)
            {
                await ShowMainMenuAsync();
            }
            else
            {
                //Call base compression method
            }
        }

        private async Task ProcessMainMenuDecompressOptionAsync()
        {
            Console.Clear();

            //Сформировать текст сообщения при выборе пункта меню "Указать файл для распаковки"
            //на основании настроек пользователя
            Console.WriteLine(
                MainMenuOptions.Decompress
                    .GetLocalizedDescription(
                        _userSettingService.GetCurrentSettings(),
                        "MainMenuOptionDescriptions"));

            var mainMenuInputPrefix = GetMainMenuInputPrefixBySettings(
                _userSettingService
                    .GetCurrentSettings());

            var compressMenuConstantsType = AssemblyHelper
                .GetTypeByNameAndSettings(
                    "CompressMenuConstants",
                    _userSettingService.GetCurrentSettings());

            var fileNotFoundMessage = compressMenuConstantsType
                .GetConstString("FileNotFoundMessage");

            bool anyValidFilePathTyped = false;
            string sourceFileName = string.Empty;

            while (!anyValidFilePathTyped)
            {
                Console.Write(mainMenuInputPrefix);
                if (TryGetFileByPath(
                    Console.ReadLine(),
                    out string fileName))
                {
                    anyValidFilePathTyped = true;
                    sourceFileName = fileName;
                }
                else
                {
                    Console.WriteLine(fileNotFoundMessage);
                }
            }

            if (sourceFileName == null)
            {
                await ShowMainMenuAsync();
            }
            else
            {
                //Call base decompression method
            }
        }

        private bool TryGetFileByPath(
            string filePath, 
            out string fileName)
        {
            if (filePath.Replace(" ", "") == 0.ToString())
            {
                fileName = null;
                return true;
            }

            fileName = filePath;

            if (File.Exists(filePath))
                return true;

            string normalizedFilePath = filePath.Normalize();

            if(File.Exists(normalizedFilePath))
            {
                fileName = normalizedFilePath;
                return true;
            }

            normalizedFilePath = normalizedFilePath.Replace(" ", "");

            if(File.Exists(normalizedFilePath))
            {
                fileName = normalizedFilePath;
                return true;
            }

            return false;
        }

        private async Task ProcessMainMenuChangeSettingsOptionAsync()
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
                await ShowMainMenuAsync();
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

        private async Task ProcessChangeSettingsMenuLanguageOptionAsync()
        {
            Console.Clear();

            var currentSettings = _userSettingService.GetCurrentSettings();

            Console.WriteLine(
                ChangeSettingsMenuOptions.Language
                    .GetLocalizedDescription(
                        currentSettings,
                        "UserSettingPropertyDescriptions"));

            var changeSettingsMenuConstantsType = AssemblyHelper
                .GetTypeByNameAndSettings(
                    "ChangeSettingsMenuConstants",
                    currentSettings);

            Console.WriteLine(changeSettingsMenuConstantsType
                .GetConstString("ChangeSettingsLanguageOptionTitle"));

            foreach (Languages option in Enum.GetValues(typeof(Languages)))
            {
                var description = option.GetBaseDescription();

                Console.WriteLine($"({(int)option}) {description}");
            }

            var currentValuePrefix = changeSettingsMenuConstantsType
                .GetConstString("ChangeSettingsShowCurrentValueMessage");

            var currentValueSuffix = currentSettings
                .Language
                .GetBaseDescription();

            Console.WriteLine($"{currentValuePrefix}{currentValueSuffix}");

            var mainMenuInputPrefix = GetMainMenuInputPrefixBySettings(
                _userSettingService
                    .GetCurrentSettings());

            bool anyValidLanguageValueSelected = false;
            Languages newLanguageValue = default(Languages);

            //Получить корректный ввод от пользователя программы
            while (!anyValidLanguageValueSelected)
            {
                Console.Write(mainMenuInputPrefix);
                if (TryGetLanguageValueFromString(
                    Console.ReadLine(),
                    out Languages value))
                {
                    anyValidLanguageValueSelected = true;
                    newLanguageValue = value;
                }
            }

            if ((int)newLanguageValue == 0)
                await ProcessMainMenuChangeSettingsOptionAsync();
            else
            {
                currentSettings.Language = newLanguageValue;

                //Сохранение новых настроек
                _userSettingService.SetCurrentSettings(currentSettings);

                Console.WriteLine(changeSettingsMenuConstantsType
                    .GetConstString("ChangeSettingsSuccessMessage"));

                Console.Write(mainMenuInputPrefix);
                Console.ReadLine();

                await ProcessMainMenuChangeSettingsOptionAsync();
            }
        }

        private async Task ProcessChangeSettingsMenuShowProgressOptionAsync()
        {
            Console.Clear();

            var currentSettings = _userSettingService.GetCurrentSettings();

            Console.WriteLine(
                ChangeSettingsMenuOptions.ShowProgress
                    .GetLocalizedDescription(
                        currentSettings,
                        "UserSettingPropertyDescriptions"));

            var changeSettingsMenuConstantsType = AssemblyHelper
                .GetTypeByNameAndSettings(
                    "ChangeSettingsMenuConstants",
                    currentSettings);

            Console.WriteLine(changeSettingsMenuConstantsType
                .GetConstString("ChangeSettingsShowProgressOptionTitle"));

            foreach (BoolValues option in Enum.GetValues(typeof(BoolValues)))
            {
                var description = option.GetLocalizedDescription(
                    _userSettingService
                        .GetCurrentSettings(),
                    "FlagValueDescriptions");

                Console.WriteLine($"({(int)option}) {description}");
            }

            var currentValuePrefix = changeSettingsMenuConstantsType
                .GetConstString("ChangeSettingsShowCurrentValueMessage");

            var currentValueSuffix = currentSettings
                .ShowProgress
                .ToBoolValues()
                .GetLocalizedDescription(
                currentSettings,
                "FlagValueDescriptions");

            Console.WriteLine($"{currentValuePrefix}{currentValueSuffix}");

            var mainMenuInputPrefix = GetMainMenuInputPrefixBySettings(
                _userSettingService
                    .GetCurrentSettings());

            bool anyValidShowProgressFlagValueSelected = false;
            BoolValues newShowProgressFlagValue = default(BoolValues);

            //Получить корректный ввод от пользователя программы
            while (!anyValidShowProgressFlagValueSelected)
            {
                Console.Write(mainMenuInputPrefix);
                if (TryGetShowProgressFlagValueFromString(
                    Console.ReadLine(),
                    out BoolValues flagValue))
                {
                    anyValidShowProgressFlagValueSelected = true;
                    newShowProgressFlagValue = flagValue;
                }
            }

            if ((int)newShowProgressFlagValue == 0)
                await ProcessMainMenuChangeSettingsOptionAsync();
            else
            {
                currentSettings.ShowProgress = newShowProgressFlagValue.ToBool();

                //Сохранение новых настроек
                _userSettingService.SetCurrentSettings(currentSettings);

                Console.WriteLine(changeSettingsMenuConstantsType
                    .GetConstString("ChangeSettingsSuccessMessage"));

                Console.Write(mainMenuInputPrefix);
                Console.ReadLine();

                await ProcessMainMenuChangeSettingsOptionAsync();
            }
        }

        private async Task ProcessChangeSettingsMenuProgressBarDelayOptionAsync()
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
                await ProcessMainMenuChangeSettingsOptionAsync();
            else
            {
                currentSettings.ProgressBarDelay = newDelayValue;

                //Сохранение новых настроек
                _userSettingService.SetCurrentSettings(currentSettings);

                Console.WriteLine(changeSettingsMenuConstantsType
                    .GetConstString("ChangeSettingsSuccessMessage"));

                Console.Write(mainMenuInputPrefix);
                Console.ReadLine();

                await ProcessMainMenuChangeSettingsOptionAsync();
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

        private bool TryGetShowProgressFlagValueFromString(
            string input,
            out BoolValues flagValue)
        {
            flagValue = default(BoolValues);

            string normalizedInput = input.Replace(" ", "");

            if (!UIHelper.ValidateInput(normalizedInput))
                return false;

            if (BoolValues.TryParse(
                normalizedInput,
                out flagValue))
            {
                if ((int)flagValue == 0)
                    return true;

                if (flagValue.IsValid())
                    return true;
            }

            return false;
        }

        private bool TryGetLanguageValueFromString(
            string input,
            out Languages language)
        {
            language = default(Languages);

            string normalizedInput = input.Replace(" ", "");

            if (!UIHelper.ValidateInput(normalizedInput))
                return false;

            if (ChangeSettingsMenuOptions.TryParse(
                normalizedInput,
                out language))
            {
                if ((int)language == 0)
                    return true;

                if (language.IsValid())
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
