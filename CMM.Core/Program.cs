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

            MainMenuService mainMenuService = new(userSettingService);

            var mainMenuTask = mainMenuService.ShowMainMenuAsync(false);

            mainMenuTask.Wait();
        }
    }
}