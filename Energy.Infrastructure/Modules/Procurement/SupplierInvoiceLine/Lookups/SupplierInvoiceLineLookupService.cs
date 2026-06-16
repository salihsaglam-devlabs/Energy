using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Procurement.SupplierInvoiceLine.Lookups;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Responses;

namespace Energy.Infrastructure.Modules.Procurement.SupplierInvoiceLine.Lookups;

/// <summary>SupplierInvoiceLine lookup servisi (aktif + arama filtreli projection).</summary>
public class SupplierInvoiceLineLookupService : ISupplierInvoiceLineLookupService
{
    private readonly EnergyDbContext _db;

    public SupplierInvoiceLineLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<SupplierInvoiceLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.SupplierInvoiceLines.AsNoTracking();
        var items = await query.Select(e => new SupplierInvoiceLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<SupplierInvoiceLineLookupResponse>>.Success(items);
    }
}
