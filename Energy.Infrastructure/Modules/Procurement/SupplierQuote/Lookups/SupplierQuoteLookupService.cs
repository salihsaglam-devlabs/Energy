using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Procurement.SupplierQuote.Lookups;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Responses;

namespace Energy.Infrastructure.Modules.Procurement.SupplierQuote.Lookups;

/// <summary>SupplierQuote lookup servisi (aktif + arama filtreli projection).</summary>
public class SupplierQuoteLookupService : ISupplierQuoteLookupService
{
    private readonly EnergyDbContext _db;

    public SupplierQuoteLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<SupplierQuoteLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.SupplierQuotes.AsNoTracking();
        var items = await query.Select(e => new SupplierQuoteLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<SupplierQuoteLookupResponse>>.Success(items);
    }
}
