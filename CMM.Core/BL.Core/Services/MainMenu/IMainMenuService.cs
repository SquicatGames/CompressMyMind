using CMM.Core.BL.Core.Common.Menu;
using CMM.Core.BL.Core.Common.Settings;

namespace CMM.Core.BL.Core.Services.MainMenu
{
    /// <summary>
    /// Интерфейс сервиса управления главным меню программы
    /// </summary>
    internal interface IMainMenuService
    {
        /// <summary>
        /// Отобразить главное меню программы
        /// </summary>
        /// <param name="cleanConsole">Очистить консоль перед выводом меню</param>
        /// <returns></returns>
        Task ShowMainMenuAsync(bool cleanConsole = true);

        /// <summary>
        /// Выполнить действия, выбранные пользователем в главном меню
        /// </summary>
        /// <param name="option">Выбранный пункт главного меню</param>
        /// <returns></returns>
        Task ProcessMainMenuOptionAsync(MainMenuOptions option);

        /// <summary>
        /// Выполнить действия, выбранные пользователем в меню редактирования настроек
        /// </summary>
        /// <param name="option">Выбранный пункт меню редактирования настроек</param>
        /// <returns></returns>
        Task ProcessChangeSettingsMenuOptionAsync(ChangeSettingsMenuOptions option);
    }
}
