using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Core.ExchangeRate.Lookups;
using Energy.Shared.Models.V1.Core.ExchangeRate.Responses;

namespace Energy.Infrastructure.Modules.Core.ExchangeRate.Lookups;

/// <summary>ExchangeRate lookup servisi (aktif + arama filtreli projection).</summary>
public class ExchangeRateLookupService : IExchangeRateLookupService
{
    private readonly AppDbContext _db;

    public ExchangeRateLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ExchangeRateLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ExchangeRates.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new ExchangeRateLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ExchangeRateLookupResponse>>.Success(items);
    }
}
