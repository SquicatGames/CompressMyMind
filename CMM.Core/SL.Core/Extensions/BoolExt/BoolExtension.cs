﻿using CMM.Core.BL.Core.Common;

namespace CMM.Core.SL.Core.Extensions.BoolExt
{
    /// <summary>
    /// Расширение типа bool
    /// </summary>
    internal static class BoolExtension
    {
        /// <summary>
        /// Преобразование значение типа bool в значение перечисления типа BoolValues
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BoolValues ToBoolValues(this bool value)
        {
            return value switch
            {
                true => BoolValues.Yes,
                false => BoolValues.No
            };
        }
    }
}
