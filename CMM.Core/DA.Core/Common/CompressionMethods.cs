namespace CMM.Core.DA.Core.Common
{
    /// <summary>
    /// Перечисление используемых методов сжатия
    /// </summary>
    enum CompressionMethods
    {
        /// <summary>
        /// Сжатие не используется
        /// </summary>
        None = 0,

        /// <summary>
        /// Сжатие методом базовых повторов
        /// </summary>
        RC = 1,

        /// <summary>
        /// Сжатие прямым кодированием по Хаффману с группой 8 бит
        /// </summary>
        HC1 = 2,

        /// <summary>
        /// Невзвешенное словарное кодирование с группой 32 бит
        /// </summary>
        DC4 = 3
    }
}
