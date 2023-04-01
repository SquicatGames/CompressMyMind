using System.ComponentModel;

namespace CMM.Core.BL.Core.Common
{
    /// <summary>
    /// Перечисление поддерживаемых языков пользовательского интерфейса
    /// Атрибут Desctiption хранит самоназвание языка
    /// </summary>
    enum Languages
    {
        [Description("English")]
        English = 0,

        [Description("Русский")]
        Russian = 1
    }
}
