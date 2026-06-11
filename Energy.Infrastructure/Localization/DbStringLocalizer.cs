using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Energy.Infrastructure.Localization;

/// <summary>
/// <see cref="IStringLocalizer"/> implementation that checks the in-memory
/// database cache first and falls back to the wrapped resx-backed localizer
/// when the (culture, key) pair has no override stored in the database.
/// </summary>
public sealed class DbStringLocalizer : IStringLocalizer
{
    private readonly IStringLocalizer _resxFallback;
    private readonly LocalizationCache _cache;

    public DbStringLocalizer(IStringLocalizer resxFallback, LocalizationCache cache)
    {
        _resxFallback = resxFallback;
        _cache = cache;
    }

    public LocalizedString this[string name] => Resolve(name, arguments: null);

    public LocalizedString this[string name, params object[] arguments] => Resolve(name, arguments);

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        => _resxFallback.GetAllStrings(includeParentCultures);

    private LocalizedString Resolve(string name, object[]? arguments)
    {
        var culture = CultureInfo.CurrentUICulture.Name;

        // 1) DB cache for the requested culture (e.g. "tr-TR").
        if (_cache.TryGet(culture, name, out var dbValue) && dbValue is not null)
        {
            return Format(name, dbValue, arguments);
        }

        // 2) DB cache for the parent culture (e.g. "tr") and the invariant culture.
        var parent = CultureInfo.CurrentUICulture.Parent.Name;
        if (!string.IsNullOrEmpty(parent)
            && _cache.TryGet(parent, name, out dbValue)
            && dbValue is not null)
        {
            return Format(name, dbValue, arguments);
        }

        if (_cache.TryGet(string.Empty, name, out dbValue) && dbValue is not null)
        {
            return Format(name, dbValue, arguments);
        }

        // 3) Standard resx pipeline.
        return arguments is null || arguments.Length == 0
            ? _resxFallback[name]
            : _resxFallback[name, arguments];
    }

    private static LocalizedString Format(string name, string value, object[]? arguments)
    {
        var formatted = arguments is null || arguments.Length == 0
            ? value
            : string.Format(CultureInfo.CurrentCulture, value, arguments);

        return new LocalizedString(name, formatted, resourceNotFound: false);
    }
}

