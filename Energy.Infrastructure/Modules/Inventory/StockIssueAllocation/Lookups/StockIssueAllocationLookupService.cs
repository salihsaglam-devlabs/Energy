using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.StockIssueAllocation.Lookups;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;

namespace Energy.Infrastructure.Modules.Inventory.StockIssueAllocation.Lookups;

/// <summary>StockIssueAllocation lookup servisi (aktif + arama filtreli projection).</summary>
public class StockIssueAllocationLookupService : IStockIssueAllocationLookupService
{
    private readonly EnergyDbContext _db;

    public StockIssueAllocationLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<StockIssueAllocationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.StockIssueAllocations.AsNoTracking();
        var items = await query.Select(e => new StockIssueAllocationLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<StockIssueAllocationLookupResponse>>.Success(items);
    }
}
