namespace CMM.Core.BL.Core.Common.Localization.EN
{
    /// <summary>
    /// Класс основных констант пользовательского интерфейса (язык: Английский)
    /// </summary>
    internal static class UIConstantsEN
    {
        public const string Greeting = "Welcome to CompressMyMind!";
        public const string CMMDescription = "This program provides easy access to most effective text compression algoritms";
        public const string CMMVersion = "Version %HighVersion%.%LowVersion% Released: %BuildDate%";
        public const string Copyright = "MIT Stefanov Anton (SquiCat)";

        public const string MainMenuInputPrefix = "->";

        public const string CompressionSuccessMessage = "File compression is successfully done! \nPress Enter to go back to Main menu";
        public const string CompressionErrorMessagePattern = "An error occured while compressing file:\n%Message%\nPress Enter to go back to Main menu";

        public const string DecompressionSuccessMessage = "File decompression is successfully done! \nPress Enter to go back to Main menu";
        public const string DecompressionErrorMessagePattern = "An error occured while decompressing file:\n%Message%\nPress Enter to go back to Main menu";

        public const string FileProcessingStateMessagePattern = "Processed %ProcessedBytes% bytes";
    }
}
