using Microsoft.Extensions.Localization;

namespace Energy.Localization;

public static class StringLocalizerExtensions
{
    public static string GetText(this IStringLocalizer<SharedResource> localizer, string key)
    {
        return localizer[key].Value;
    }

    public static string GetText(this IStringLocalizer localizer, string key, string fallback)
    {
        var value = localizer[key];
        return value.ResourceNotFound ? fallback : value.Value;
    }
}
