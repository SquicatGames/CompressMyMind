using System.ComponentModel;

namespace CMM.Core.BL.Core.Common.Menu
{
    /// <summary>
    /// Перечисление пунктов главного меню
    /// </summary>
    enum MainMenuOptions
    {
        [Description("MainMenuCompressionOptionTitle")]
        Compress = 1,

        [Description("MainMenuDecompressionOptionTitle")]
        Decompress = 2,

        [Description("MainMenuChangeSettingsOptionTitle")]
        ChangeSettings = 3,

        [Description("MainMenuQuitOptionTitle")]
        Quit = 4
    }
}
