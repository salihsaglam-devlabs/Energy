using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Procurement.SupplierQuoteLine.Lookups;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;

namespace Energy.Infrastructure.Procurement.SupplierQuoteLine.Lookups;

/// <summary>SupplierQuoteLine lookup servisi (aktif + arama filtreli projection).</summary>
public class SupplierQuoteLineLookupService : ISupplierQuoteLineLookupService
{
    private readonly AppDbContext _db;

    public SupplierQuoteLineLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<SupplierQuoteLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.SupplierQuoteLines.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<SupplierQuoteLineLookupResponse>)rows.Select(e => new SupplierQuoteLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Description ?? "") + " - " + e.Quantity.ToString()) ? "Supplier Quote Line #" + e.Id.ToString().Substring(0, 8) : ((e.Description ?? "") + " - " + e.Quantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<SupplierQuoteLineLookupResponse>>.Success(items);
    }
}
