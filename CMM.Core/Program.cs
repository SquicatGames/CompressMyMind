using CMM.Core.BL.Core.Common.Menu;
using CMM.Core.BL.Core.Helpers;
using CMM.Core.BL.Core.Services.MainMenu;
using CMM.Core.BL.Core.Services.UserSettings;

namespace CMM.Core
{
    internal class Program
    {
        //Отключить отображение предупреждений
        const bool _disableWarnings = false;

        static void Main(string[] args)
        {
            //Инициализация класса настроек пользователя программы
            UserSettingService userSettingService = new();

            var currentSettings = userSettingService
                        .GetCurrentSettings();

            var greetingMessage = UIHelper.GetGreetingMessageBySettings(currentSettings);

            //Отображение текста приветствия с общей информацией о программе (учитывает локализацию)
            Console.WriteLine(greetingMessage);
            Console.WriteLine();

            if (!_disableWarnings)
            {
                //Получение строкового представления списка предупреждений, сформированного при загрузке настроек пользователя
                var settingLoadWarnings = userSettingService.GetFileSettingLoadWarnings();

                if (settingLoadWarnings != default)
                {
                    Console.WriteLine(settingLoadWarnings);
                    Console.WriteLine();
                }
            }

            var mainMenuData = UIHelper.GetMainMenuStringBySettings(currentSettings);
            var mainMenuInputPrefix = UIHelper.GetMainMenuInputPrefixBySettings(currentSettings);

            //Отображение главного меню программы
            Console.WriteLine(mainMenuData);

            bool anyMainMenuOptionSelected = false;
            MainMenuOptions selectedOption = default(MainMenuOptions);

            while(!anyMainMenuOptionSelected)
            {
                Console.Write(mainMenuInputPrefix);
                if(UIHelper.TryGetMenuOptionFromString(
                    Console.ReadLine(),
                    out MainMenuOptions option))
                {
                    anyMainMenuOptionSelected = true;
                    selectedOption = option;
                }
            }

            MainMenuService mainMenuService = new(userSettingService);

            //Выполнить действия в соответствие с выбором пользователя
            mainMenuService.ProcessMainMenuOptionAsync(selectedOption);
        }
    }
}