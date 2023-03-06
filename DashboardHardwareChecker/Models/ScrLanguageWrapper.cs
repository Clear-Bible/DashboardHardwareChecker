namespace DashboardHardwareChecker.Models
{
    public class ScrLanguageWrapper
    {

        public string FontFamily { get; set; } = "Segoe UI";

        public float Size { get; set; } = 13;

        public bool IsRtol { get; set; }

        public ScrLanguage? Language { get; set; }

    }

    public class ScrLanguage
    {
        public string? DisplayName { get; set; }

        public string? LanguageTag { get; set; }

    }
}
