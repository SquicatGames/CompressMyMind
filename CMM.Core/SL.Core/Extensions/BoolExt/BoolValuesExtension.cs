using CMM.Core.BL.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.SL.Core.Extensions.BoolExt
{
    /// <summary>
    /// Расширение типа BoolValues
    /// </summary>
    internal static class BoolValuesExtension
    {
        /// <summary>
        /// Преобразование значение типа перечисления BoolValues в значение типа bool
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this BoolValues value)
        {
            return value switch
            {
                BoolValues.Yes => true,
                BoolValues.No => false,
                _ => false,
            };
        }
    }
}
