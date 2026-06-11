using System.Collections.Concurrent;
using Energy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Energy.Infrastructure.Localization;

/// <summary>
/// In-memory cache of database-backed localization overrides. Loaded lazily
/// on first access and kept up to date by <see cref="DatabaseLocalizationService"/>
/// whenever entries are upserted or deleted.
/// </summary>
public sealed class LocalizationCache
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private ConcurrentDictionary<string, string>? _entries;

    public LocalizationCache(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public bool TryGet(string culture, string key, out string? value)
    {
        EnsureLoaded();
        return _entries!.TryGetValue(MakeKey(culture, key), out value);
    }

    public void Set(string culture, string key, string value)
    {
        EnsureLoaded();
        _entries![MakeKey(culture, key)] = value;
    }

    public void Remove(string culture, string key)
    {
        _entries?.TryRemove(MakeKey(culture, key), out _);
    }

    /// <summary>
    /// Drops the in-memory snapshot so the next access reloads it from the database.
    /// </summary>
    public void Invalidate()
    {
        _entries = null;
    }

    private void EnsureLoaded()
    {
        if (_entries is not null)
        {
            return;
        }

        _initLock.Wait();
        try
        {
            if (_entries is not null)
            {
                return;
            }

            var dict = new ConcurrentDictionary<string, string>(StringComparer.Ordinal);
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var rows = db.Resources
                .AsNoTracking()
                .Select(entry => new { entry.Culture, entry.Key, entry.Value })
                .ToList();

            foreach (var row in rows)
            {
                dict[MakeKey(row.Culture, row.Key)] = row.Value;
            }

            _entries = dict;
        }
        finally
        {
            _initLock.Release();
        }
    }

    private static string MakeKey(string culture, string key) => $"{culture}::{key}";
}

