using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Energy.Infrastructure.Localization;

/// <summary>
/// Önce bellek içi veritabanı önbelleğini kontrol eden, (kültür, anahtar) ikilisi
/// için veritabanında bir geçersiz kılma kaydı yoksa sarmalanan resx tabanlı
/// yerelleştiriciye geri düşen <see cref="IStringLocalizer"/> uygulaması.
/// </summary>
public sealed class DbStringLocalizer : IStringLocalizer
{
    private readonly IStringLocalizer _resxFallback;
    private readonly LocalizationCache _cache;

    /// <summary>Resx yedeğini ve veritabanı önbelleğini enjekte eder.</summary>
    public DbStringLocalizer(IStringLocalizer resxFallback, LocalizationCache cache)
    {
        _resxFallback = resxFallback;
        _cache = cache;
    }

    /// <summary>Verilen anahtar için yerelleştirilmiş dizeyi döndürür.</summary>
    public LocalizedString this[string name] => Resolve(name, arguments: null);

    /// <summary>Verilen anahtar ve biçimlendirme argümanları için yerelleştirilmiş dizeyi döndürür.</summary>
    public LocalizedString this[string name, params object[] arguments] => Resolve(name, arguments);

    /// <summary>Tüm yerelleştirilmiş dizeleri resx yedeğinden döndürür.</summary>
    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        => _resxFallback.GetAllStrings(includeParentCultures);

    /// <summary>Bir anahtarı önce veritabanı önbelleğinden, sonra resx yedeğinden çözümler.</summary>
    private LocalizedString Resolve(string name, object[]? arguments)
    {
        var culture = CultureInfo.CurrentUICulture.Name;

        // 1) İstenen kültür için veritabanı önbelleği (ör. "tr-TR").
        if (_cache.TryGet(culture, name, out var dbValue) && dbValue is not null)
        {
            return Format(name, dbValue, arguments);
        }

        // 2) Üst kültür (ör. "tr") ve invariant kültür için veritabanı önbelleği.
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

        // 3) Standart .resx hattı.
        return arguments is null || arguments.Length == 0
            ? _resxFallback[name]
            : _resxFallback[name, arguments];
    }

    /// <summary>Değeri biçimlendirme argümanlarıyla (varsa) işleyip yerelleştirilmiş dize üretir.</summary>
    private static LocalizedString Format(string name, string value, object[]? arguments)
    {
        var formatted = arguments is null || arguments.Length == 0
            ? value
            : string.Format(CultureInfo.CurrentCulture, value, arguments);

        return new LocalizedString(name, formatted, resourceNotFound: false);
    }
}

