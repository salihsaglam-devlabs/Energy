using System.Globalization;
using System.Resources;

namespace Energy.Localization;

public static class LocalizationText
{
    private static readonly ResourceManager ResourceManager =
        new("Energy.Localization.Resources.SharedResource", typeof(SharedResource).Assembly);

    public static string Get(string key, string fallback)
    {
        var value = ResourceManager.GetString(key, CultureInfo.CurrentUICulture);
        return string.IsNullOrWhiteSpace(value) ? fallback : value;
    }
}

