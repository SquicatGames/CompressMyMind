using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.BL.Core
{
    /// <summary>
    /// Описание текущей сборки
    /// </summary>
    internal static class AppInfo
    {
        /// <summary>
        /// Старшая компонента версии
        /// </summary>
        public const int HighVersion = 0;

        /// <summary>
        /// Младшая компонента версии
        /// </summary>
        public const int LowVersion = 1;

        /// <summary>
        /// Дата текущей сборки
        /// </summary>
        public static DateOnly BuildDate = new DateOnly(2023, 03, 31);
    }
}
