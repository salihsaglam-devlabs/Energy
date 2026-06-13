using Microsoft.Extensions.Localization;

namespace Energy.Infrastructure.Localization;

/// <summary>
/// DI'dan çözülen her <see cref="IStringLocalizer"/>, .resx kaynaklarına geri
/// düşmeden önce <see cref="LocalizationCache"/>'i kontrol etsin diye, çatının
/// <see cref="ResourceManagerStringLocalizerFactory"/> tipini süsleyen (decorator) fabrika.
/// </summary>
public sealed class DbStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly ResourceManagerStringLocalizerFactory _innerFactory;
    private readonly LocalizationCache _cache;

    public DbStringLocalizerFactory(
        ResourceManagerStringLocalizerFactory innerFactory,
        LocalizationCache cache)
    {
        _innerFactory = innerFactory;
        _cache = cache;
    }

    public IStringLocalizer Create(Type resourceSource)
        => new DbStringLocalizer(_innerFactory.Create(resourceSource), _cache);

    public IStringLocalizer Create(string baseName, string location)
        => new DbStringLocalizer(_innerFactory.Create(baseName, location), _cache);
}

