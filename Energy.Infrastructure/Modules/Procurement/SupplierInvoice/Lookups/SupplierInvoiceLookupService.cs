using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Procurement.SupplierInvoice.Lookups;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Responses;

namespace Energy.Infrastructure.Modules.Procurement.SupplierInvoice.Lookups;

/// <summary>SupplierInvoice lookup servisi (aktif + arama filtreli projection).</summary>
public class SupplierInvoiceLookupService : ISupplierInvoiceLookupService
{
    private readonly EnergyDbContext _db;

    public SupplierInvoiceLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<SupplierInvoiceLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.SupplierInvoices.AsNoTracking();
        var items = await query.Select(e => new SupplierInvoiceLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<SupplierInvoiceLookupResponse>>.Success(items);
    }
}
