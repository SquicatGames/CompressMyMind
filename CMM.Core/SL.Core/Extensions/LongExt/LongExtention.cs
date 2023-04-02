using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.SL.Core.Extensions.IntExt
{
    /// <summary>
    /// Расширение класса long
    /// </summary>
    internal static class LongExtention
    {
        /// <summary>
        /// Получить строковое представление целого числа в региональном формате
        /// (группировка по 3 цифры)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToLocalizedString(this long value)
        {
            char[] source = value.ToString().ToCharArray();
            string result = string.Empty;

            for(int i=0; i < source.Length; i++)
            {
                result += source[i];
                if ((source.Length - i) % 3 == 1)
                    result += " ";
            }

            return result.TrimEnd();
        }
    }
}
