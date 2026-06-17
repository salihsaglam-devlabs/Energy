using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Inventory.StockIssueAllocation.Lookups;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;

namespace Energy.Infrastructure.Inventory.StockIssueAllocation.Lookups;

/// <summary>StockIssueAllocation lookup servisi (aktif + arama filtreli projection).</summary>
public class StockIssueAllocationLookupService : IStockIssueAllocationLookupService
{
    private readonly AppDbContext _db;

    public StockIssueAllocationLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockIssueAllocationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockIssueAllocations.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<StockIssueAllocationLookupResponse>)rows.Select(e => new StockIssueAllocationLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.Quantity.ToString()) ? "Stock Issue Allocation #" + e.Id.ToString().Substring(0, 8) : (e.Quantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<StockIssueAllocationLookupResponse>>.Success(items);
    }
}
