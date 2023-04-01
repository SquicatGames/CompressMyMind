using CMM.Core.BL.Core.Common.Menu;

namespace CMM.Core.BL.Core.Services.MainMenu
{
    /// <summary>
    /// Интерфейс сервиса управления главным меню программы
    /// </summary>
    internal interface IMainMenuService
    {
        /// <summary>
        /// Выполнить действия, выбранные пользователем
        /// </summary>
        /// <param name="option">Выбранный пункт главного меню</param>
        /// <returns></returns>
        Task ProcessMainMenuOptionAsync(MainMenuOptions option);
    }
}
