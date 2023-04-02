using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.BL.Core.Common
{
    /// <summary>
    /// Допустимые значения флагов типа bool
    /// </summary>
    enum BoolValues
    {
        [Description("FlagValueYesDescription")]
        Yes = 1,

        [Description("FlagValueNoDescription")]
        No = 2
    }
}
