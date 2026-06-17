using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Energy.Infrastructure.Persistence;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Core.Currency.Lookups;
using Energy.Shared.Models.V1.Core.Currency.Responses;

namespace Energy.Infrastructure.Core.Currency.Lookups;

/// <summary>Currency lookup servisi (aktif + arama filtreli projection).</summary>
public class CurrencyLookupService : ICurrencyLookupService
{
    private readonly AppDbContext _db;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public CurrencyLookupService(AppDbContext db, IStringLocalizer<SharedResource> localizer)
    {
        _db = db;
        _localizer = localizer;
    }

    public async Task<BaseResponse<IReadOnlyList<CurrencyLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Currencies.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var rows = await query
            .OrderBy(e => e.Code)
            .Select(e => new { e.Id, e.Code, e.Name, e.IsActive })
            .ToListAsync(ct);
        // Name bir yerelleştirme anahtarı (ör. "Currencies.TRY.Name") olabilir; çöz.
        var items = (IReadOnlyList<CurrencyLookupResponse>)rows.Select(e =>
        {
            var resolvedName = ResolveName(e.Name);
            return new CurrencyLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = resolvedName,
                DisplayName = e.Code + " - " + resolvedName,
                IsActive = e.IsActive
            };
        }).ToList();
        return BaseResponse<IReadOnlyList<CurrencyLookupResponse>>.Success(items);
    }

    // Değer bir kaynak anahtarı görünümündeyse (örn. "Currencies.TRY.Name") localizer
    // ile çöz; aksi halde ham metni döndür.
    private string ResolveName(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return string.Empty;
        if (raw.Contains('.') && !raw.Contains(' '))
        {
            var localized = _localizer[raw];
            if (!localized.ResourceNotFound && !string.IsNullOrWhiteSpace(localized.Value))
                return localized.Value;
        }
        return raw;
    }
}
