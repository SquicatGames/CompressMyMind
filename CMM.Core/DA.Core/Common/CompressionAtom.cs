using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.DA.Core.Common
{
    /// <summary>
    /// Атомарный элемент сжатого представления данных
    /// </summary>
    internal record CompressionAtom
    {
        /// <summary>
        /// Числовое представление кода подстроки
        /// </summary>
        public int Code;

        /// <summary>
        /// Хранимая длина кода (в битах)
        /// </summary>
        public int CodeLen;
    }
}
