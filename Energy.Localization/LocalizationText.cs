using System.Globalization;
using System.Resources;

namespace Energy.Localization;

/// <summary>
/// Gömülü .resx kaynaklarından doğrudan (DI olmadan) metin okumak için basit
/// yardımcı. Anahtar bulunamazsa verilen yedek (fallback) metni döndürür.
/// </summary>
public static class LocalizationText
{
    /// <summary>Gömülü SharedResource kaynaklarına erişen yönetici (resource manager).</summary>
    private static readonly ResourceManager ResourceManager =
        new("Energy.Localization.Resources.SharedResource", typeof(SharedResource).Assembly);

    /// <summary>
    /// Geçerli arayüz kültürü için anahtarın değerini döndürür; bulunamaz veya
    /// boşsa <paramref name="fallback"/> değerini verir.
    /// </summary>
    public static string Get(string key, string fallback)
    {
        var value = ResourceManager.GetString(key, CultureInfo.CurrentUICulture);
        return string.IsNullOrWhiteSpace(value) ? fallback : value;
    }
}
