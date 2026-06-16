using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Procurement.SupplierQuote.Lookups;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Responses;

namespace Energy.Infrastructure.Modules.Procurement.SupplierQuote.Lookups;

/// <summary>SupplierQuote lookup servisi (aktif + arama filtreli projection).</summary>
public class SupplierQuoteLookupService : ISupplierQuoteLookupService
{
    private readonly AppDbContext _db;

    public SupplierQuoteLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<SupplierQuoteLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.SupplierQuotes.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.QuoteNo.Contains(search));
        var items = await query
            .OrderBy(e => e.QuoteNo)
            .Select(e => new SupplierQuoteLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.QuoteNo,
                DisplayName = e.QuoteNo,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<SupplierQuoteLookupResponse>>.Success(items);
    }
}
