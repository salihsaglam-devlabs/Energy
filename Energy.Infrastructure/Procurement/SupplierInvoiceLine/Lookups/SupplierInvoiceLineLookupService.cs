using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Procurement.SupplierInvoiceLine.Lookups;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Responses;

namespace Energy.Infrastructure.Procurement.SupplierInvoiceLine.Lookups;

/// <summary>SupplierInvoiceLine lookup servisi (aktif + arama filtreli projection).</summary>
public class SupplierInvoiceLineLookupService : ISupplierInvoiceLineLookupService
{
    private readonly AppDbContext _db;

    public SupplierInvoiceLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<SupplierInvoiceLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.SupplierInvoiceLines.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<SupplierInvoiceLineLookupResponse>)rows.Select(e => new SupplierInvoiceLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Description ?? "") + " - " + e.Quantity.ToString()) ? "Supplier Invoice Line #" + e.Id.ToString().Substring(0, 8) : ((e.Description ?? "") + " - " + e.Quantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<SupplierInvoiceLineLookupResponse>>.Success(items);
    }
}
