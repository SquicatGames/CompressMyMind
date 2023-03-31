using CMM.Core.BL.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.BL.Core.Models.Settings
{
    /// <summary>
    /// Представление основных настроек пользователя
    /// </summary>
    internal class UserSettingsModel
    {
        /// <summary>
        /// Язык интерфейса
        /// </summary>
        public Languages Language { get; set; }

        /// <summary>
        /// Признак отображения прогресса сжатия данных
        /// </summary>
        public bool ShowProgress { get; set; }

        /// <summary>
        /// Интервал обновления данных строки состояния (в мсек)
        /// </summary>
        public int ProgressBarDelay { get; set; }
    }
}
