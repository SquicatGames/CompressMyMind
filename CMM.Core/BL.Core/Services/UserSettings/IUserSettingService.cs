using CMM.Core.BL.Core.Common.Environment;
using CMM.Core.BL.Core.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Core.BL.Core.Services.UserSettings
{
    /// <summary>
    /// Интерфейс сервиса управления настроками пользователя программы
    /// </summary>
    internal interface IUserSettingService
    {
        /// <summary>
        /// Загрузить настройки пользователя из файла
        /// </summary>
        /// <param name="path">Расположение и имя файла с настройками (ожидается формат JSON)</param>
        void LoadSettingsFromFile(string path = DefaultSettings.DFSettingsPath);

        /// <summary>
        /// Получить текущие настройки сотрудника
        /// </summary>
        /// <returns>Возвращает активные на момент запуска программы настройки</returns>
        UserSettingsModel GetCurrentSettings();

        /// <summary>
        /// Сформировать строковое представление списка предупреждений, возникших при загрузке
        /// файла настроек, для отображения пользователю
        /// </summary>
        /// <returns>Строка с локализованным текстом предупреждений или NULL</returns>
        string GetFileSettingLoadWarnings();
    }
}
