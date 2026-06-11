using System.Globalization;

namespace Energy.Localization;

public static class CultureConstants
{
    public const string DefaultCulture = "tr-TR";
    public const string TurkishCulture = "tr-TR";
    public const string EnglishCulture = "en-US";

    public static readonly CultureInfo[] SupportedCultures =
    [
        new(TurkishCulture),
        new(EnglishCulture)
    ];
}
