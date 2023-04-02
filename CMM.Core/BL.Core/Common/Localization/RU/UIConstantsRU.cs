namespace CMM.Core.BL.Core.Common.Localization.RU
{
    /// <summary>
    /// Класс основных констант пользовательского интерфейса (язык: Русский)
    /// </summary>
    internal static class UIConstantsRU
    {
        public const string Greeting = "Добро пожаловать в CompressMyMind!";
        public const string CMMDescription = "Эта программа предоставляет легкий доступ к наиболее эффективным алгоритмам сжатия текста";
        public const string CMMVersion = "Версия %HighVersion%.%LowVersion% Опубликована: %BuildDate%";
        public const string Copyright = "MIT лицензия Стефанов Антон (SquiCat)";

        public const string MainMenuInputPrefix = "->";

        public const string CompressionSuccessMessage = "Сжатие файла успешно завершено \nНажмите Enter чтобы вернуться в главное меню";
        public const string CompressionErrorMessagePattern = "При сжатии файла возникла ошибка:\n%Message%\nНажмите Enter чтобы вернуться в главное меню";

        public const string DecompressionSuccessMessage = "Распаковка файла успешно завершена \nНажмите Enter чтобы вернуться в главное меню";
        public const string DecompressionErrorMessagePattern = "При распаковке файла возникла ошибка:\n%Message%\nНажмите Enter чтобы вернуться в главное меню";

        public const string FileProcessingStateMessagePattern = "Обработано %ProcessedBytes% байт";
    }
}
