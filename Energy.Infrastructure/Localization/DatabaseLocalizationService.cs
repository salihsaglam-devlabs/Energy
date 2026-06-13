using Energy.Application.Localization.Services;
using Energy.Domain.Localization;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Localization.Requests;
using Energy.Shared.Models.V1.Localization.Responses;
using Microsoft.EntityFrameworkCore;

namespace Energy.Infrastructure.Localization;

/// <summary>Yerelleştirme girdilerini veritabanı üzerinden yöneten servis (önbellek ve .resx senkronizasyonu ile).</summary>
public sealed class DatabaseLocalizationService : ILocalizationService
{
    private readonly AppDbContext _dbContext;
    private readonly LocalizationCache _cache;
    private readonly ResxFileWriter _resxWriter;
    private readonly EmbeddedResourceReader _embeddedReader;

    public DatabaseLocalizationService(
        AppDbContext dbContext,
        LocalizationCache cache,
        ResxFileWriter resxWriter,
        EmbeddedResourceReader embeddedReader)
    {
        _dbContext = dbContext;
        _cache = cache;
        _resxWriter = resxWriter;
        _embeddedReader = embeddedReader;
    }

    public async Task<IReadOnlyList<LocalizationEntryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entries = await _dbContext.Resources
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return entries
            .GroupBy(entry => entry.Key, StringComparer.Ordinal)
            .Select(MapGroup)
            .OrderBy(response => response.Key, StringComparer.Ordinal)
            .ToList();
    }

    public async Task<LocalizationEntryResponse?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var entries = await _dbContext.Resources
            .AsNoTracking()
            .Where(entry => entry.Key == key)
            .ToListAsync(cancellationToken);

        return entries.Count == 0 ? null : MapGroup(entries.GroupBy(e => e.Key).Single());
    }

    public async Task<LocalizationEntryResponse> UpsertAsync(
        UpsertLocalizationEntryRequest request,
        CancellationToken cancellationToken = default)
    {
        // Validation (non-empty key, at least one value, supported culture, ...)
        // is enforced upstream by UpsertLocalizationEntryCommandValidator.
        var key = request.Key.Trim();

        var existingEntries = await _dbContext.Resources
            .Where(entry => entry.Key == key)
            .ToListAsync(cancellationToken);

        var now = DateTime.UtcNow;

        foreach (var (cultureRaw, valueRaw) in request.Values)
        {
            var culture = (cultureRaw ?? string.Empty).Trim();
            var value = valueRaw ?? string.Empty;

            var existing = existingEntries.FirstOrDefault(entry =>
                string.Equals(entry.Culture, culture, StringComparison.OrdinalIgnoreCase));

            if (existing is null)
            {
                _dbContext.Resources.Add(new Resource
                {
                    Id = Guid.NewGuid(),
                    Key = key,
                    Culture = culture,
                    Value = value,
                    CreatedAt = now
                });
            }
            else if (!string.Equals(existing.Value, value, StringComparison.Ordinal))
            {
                existing.Value = value;
                existing.UpdatedAt = now;
            }

            _cache.Set(culture, key, value);
            _resxWriter.Upsert(culture, key, value);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return (await GetByKeyAsync(key, cancellationToken))!;
    }

    public async Task<bool> DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        var entries = await _dbContext.Resources
            .Where(entry => entry.Key == key)
            .ToListAsync(cancellationToken);

        if (entries.Count == 0)
        {
            return false;
        }

        _dbContext.Resources.RemoveRange(entries);
        await _dbContext.SaveChangesAsync(cancellationToken);

        foreach (var entry in entries)
        {
            _cache.Remove(entry.Culture, entry.Key);
        }

        _resxWriter.Delete(key);
        return true;
    }

    public async Task<SeedResultResponse> ImportFromResxAsync(CancellationToken cancellationToken = default)
    {
        var resxEntries = _resxWriter.ReadAll();
        return await SeedEntriesAsync(resxEntries, forceOverwrite: false, cancellationToken);
    }

    public async Task<SeedResultResponse> SeedFromResourcesAsync(CancellationToken cancellationToken = default)
    {
        // GÖMÜLÜ kaynaklardan okur; böylece kaynak .resx dosyaları diskte olmasa bile
        // üretimde çalışır. Mevcut satırlar gömülü değerle üzerine yazılır.
        var embeddedEntries = _embeddedReader.ReadAll();
        return await SeedEntriesAsync(embeddedEntries, forceOverwrite: true, cancellationToken);
    }

    /// <summary>
    /// Verilen (kültür, anahtar, değer) üçlülerini veritabanına ekler/günceller.
    /// <paramref name="forceOverwrite"/> true olduğunda mevcut bir satır, değeri
    /// değişmemiş olsa bile her zaman yeniden damgalanır.
    /// </summary>
    private async Task<SeedResultResponse> SeedEntriesAsync(
        IReadOnlyList<(string Culture, string Key, string Value)> entries,
        bool forceOverwrite,
        CancellationToken cancellationToken)
    {
        var added = 0;
        var updated = 0;
        var now = DateTime.UtcNow;

        if (entries.Count == 0)
        {
            var total = await _dbContext.Resources.CountAsync(cancellationToken);
            return new SeedResultResponse { Added = 0, Updated = 0, Total = total };
        }

        var existing = await _dbContext.Resources.ToListAsync(cancellationToken);
        var lookup = existing.ToDictionary(
            e => (e.Key, e.Culture),
            e => e,
            new KeyCultureComparer());

        foreach (var (culture, key, value) in entries)
        {
            if (lookup.TryGetValue((key, culture), out var row))
            {
                if (forceOverwrite || !string.Equals(row.Value, value, StringComparison.Ordinal))
                {
                    row.Value = value;
                    row.UpdatedAt = now;
                    updated++;
                }
            }
            else
            {
                _dbContext.Resources.Add(new Resource
                {
                    Id = Guid.NewGuid(),
                    Key = key,
                    Culture = culture,
                    Value = value,
                    CreatedAt = now
                });
                added++;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        _cache.Invalidate();

        var totalAfter = await _dbContext.Resources.CountAsync(cancellationToken);
        return new SeedResultResponse { Added = added, Updated = updated, Total = totalAfter };
    }

    private static LocalizationEntryResponse MapGroup(IGrouping<string, Resource> group)
    {
        return new LocalizationEntryResponse
        {
            Key = group.Key,
            Values = group.ToDictionary(
                entry => entry.Culture,
                entry => entry.Value,
                StringComparer.OrdinalIgnoreCase)
        };
    }

    private sealed class KeyCultureComparer : IEqualityComparer<(string Key, string Culture)>
    {
        public bool Equals((string Key, string Culture) x, (string Key, string Culture) y)
            => string.Equals(x.Key, y.Key, StringComparison.Ordinal)
               && string.Equals(x.Culture, y.Culture, StringComparison.OrdinalIgnoreCase);

        public int GetHashCode((string Key, string Culture) obj)
            => HashCode.Combine(obj.Key, obj.Culture.ToLowerInvariant());
    }
}

