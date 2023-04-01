using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.SL.Core.Extensions.TypeExt
{
    /// <summary>
    /// Расширение класса Type
    /// </summary>
    internal static class TypeExtension
    {
        /// <summary>
        /// Получить значение строковой константы из типа статического класса
        /// </summary>
        /// <param name="type">Тип статического класса</param>
        /// <param name="fieldName">Название поля константного типа</param>
        /// <returns></returns>
        public static string CMMGetConstString(
            this Type type,
            string fieldName)
        {
            return ((string?)type
                .GetField(fieldName)
                ?.GetRawConstantValue())
                ?? string.Empty;
        }
    }
}
