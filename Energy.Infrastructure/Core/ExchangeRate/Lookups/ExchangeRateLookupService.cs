using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Core.ExchangeRate.Lookups;
using Energy.Shared.Models.V1.Core.ExchangeRate.Responses;

namespace Energy.Infrastructure.Core.ExchangeRate.Lookups;

/// <summary>ExchangeRate lookup servisi (aktif + arama filtreli projection).</summary>
public class ExchangeRateLookupService : IExchangeRateLookupService
{
    private readonly AppDbContext _db;

    public ExchangeRateLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ExchangeRateLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ExchangeRates.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ExchangeRateLookupResponse>)rows.Select(e => new ExchangeRateLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.RateDate.ToString("yyyy-MM-dd")) ? "Exchange Rate #" + e.Id.ToString().Substring(0, 8) : (e.RateDate.ToString("yyyy-MM-dd")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ExchangeRateLookupResponse>>.Success(items);
    }
}
