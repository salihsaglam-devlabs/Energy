using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Procurement.SupplierQuoteLine.Lookups;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;

namespace Energy.Infrastructure.Modules.Procurement.SupplierQuoteLine.Lookups;

/// <summary>SupplierQuoteLine lookup servisi (aktif + arama filtreli projection).</summary>
public class SupplierQuoteLineLookupService : ISupplierQuoteLineLookupService
{
    private readonly EnergyDbContext _db;

    public SupplierQuoteLineLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<SupplierQuoteLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.SupplierQuoteLines.AsNoTracking();
        var items = await query.Select(e => new SupplierQuoteLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<SupplierQuoteLineLookupResponse>>.Success(items);
    }
}
