using Microsoft.Extensions.Localization;

namespace Energy.Localization;

/// <summary>Yerelleştirme erişimini kısaltan <see cref="IStringLocalizer"/> uzantı metotları.</summary>
public static class StringLocalizerExtensions
{
    /// <summary>Verilen anahtarın yerelleştirilmiş değerini döndürür (SharedResource üzerinden).</summary>
    public static string GetText(this IStringLocalizer<SharedResource> localizer, string key)
    {
        return localizer[key].Value;
    }

    /// <summary>
    /// Verilen anahtarın yerelleştirilmiş değerini döndürür; anahtar bulunamazsa
    /// <paramref name="fallback"/> metnini verir.
    /// </summary>
    public static string GetText(this IStringLocalizer localizer, string key, string fallback)
    {
        var value = localizer[key];
        return value.ResourceNotFound ? fallback : value.Value;
    }
}
