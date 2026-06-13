using System.Globalization;

namespace Energy.Localization;

/// <summary>Desteklenen kültürler ve varsayılan kültür için sabitler.</summary>
public static class CultureConstants
{
    /// <summary>Uygulamanın varsayılan kültürü (Türkçe).</summary>
    public const string DefaultCulture = "tr-TR";

    /// <summary>Türkçe kültür kodu.</summary>
    public const string TurkishCulture = "tr-TR";

    /// <summary>İngilizce kültür kodu.</summary>
    public const string EnglishCulture = "en-US";

    /// <summary>Uygulamanın desteklediği kültürlerin listesi.</summary>
    public static readonly CultureInfo[] SupportedCultures =
    [
        new(TurkishCulture),
        new(EnglishCulture)
    ];
}
