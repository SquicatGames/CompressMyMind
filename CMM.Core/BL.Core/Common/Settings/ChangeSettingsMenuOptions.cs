using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.BL.Core.Common.Settings
{
    /// <summary>
    /// Перечисление пунктов меню редактирования пользовательских настроек
    /// </summary>
    enum ChangeSettingsMenuOptions
    {
        [Description("LanguagePropertyDescription")]
        Language = 1,

        [Description("ShowProgressPropertyDescription")]
        ShowProgress = 2,

        [Description("ProgressBarDelayPropertyDescription")]
        ProgressBarDelay = 3
    }
}
