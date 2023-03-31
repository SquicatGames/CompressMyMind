using CMM.Core.BL.Core.Helpers;
using CMM.Core.BL.Core.Services.UserSettings;

namespace CMM.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserSettingService userSettingService = new();

            var greetingMessage = UIHelper
                .GetGreetingMessageBySettings(
                    userSettingService
                        .GetCurrentSettings()
                );

            Console.WriteLine(greetingMessage);

            var settingLoadWarnings = userSettingService.GetFileSettingLoadWarnings();

            if (settingLoadWarnings != default)
            { 
                Console.WriteLine(settingLoadWarnings);
            }

            Console.ReadLine();
        }
    }
}