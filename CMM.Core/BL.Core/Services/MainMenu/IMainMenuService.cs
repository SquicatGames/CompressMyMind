using CMM.Core.BL.Core.Common.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.BL.Core.Services.MainMenu
{
    internal interface IMainMenuService
    {
        Task ProcessMainMenuOptionAsync(MainMenuOptions option);
    }
}
