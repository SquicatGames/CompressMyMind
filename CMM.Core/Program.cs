using CMM.Core.BL.Core.Helpers;
using CMM.Core.BL.Core.Services.UserSettings;

namespace CMM.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Инициализация класса настроек пользователя программы
            UserSettingService userSettingService = new();

            var greetingMessage = UIHelper
                .GetGreetingMessageBySettings(
                    userSettingService
                        .GetCurrentSettings()
                );

            //Отображение текста приветствия с общей информацией о программе (учитывает локализацию)
            Console.WriteLine(greetingMessage);
            Console.WriteLine();

            //Получение строкового представления списка предупреждений, сформированного при загрузке настроек пользователя
            var settingLoadWarnings = userSettingService.GetFileSettingLoadWarnings();

            if (settingLoadWarnings != default)
            { 
                Console.WriteLine(settingLoadWarnings);
            }

            Console.ReadLine();
        }
    }
}