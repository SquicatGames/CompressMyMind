using System.ComponentModel;

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
