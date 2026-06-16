using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.Currency.Lookups;
using Energy.Shared.Models.V1.Core.Currency.Responses;

namespace Energy.Infrastructure.Modules.Core.Currency.Lookups;

/// <summary>Currency lookup servisi (aktif + arama filtreli projection).</summary>
public class CurrencyLookupService : ICurrencyLookupService
{
    private readonly AppDbContext _db;

    public CurrencyLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<CurrencyLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Currencies.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new CurrencyLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<CurrencyLookupResponse>>.Success(items);
    }
}
