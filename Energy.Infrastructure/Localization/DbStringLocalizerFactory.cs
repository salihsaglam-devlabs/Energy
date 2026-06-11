using Microsoft.Extensions.Localization;

namespace Energy.Infrastructure.Localization;

/// <summary>
/// Decorates the framework's <see cref="ResourceManagerStringLocalizerFactory"/>
/// so every <see cref="IStringLocalizer"/> resolved from DI checks the
/// <see cref="LocalizationCache"/> before falling back to .resx resources.
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

