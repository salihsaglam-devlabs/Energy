using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Inventory.Warehouse.Lookups;
using Energy.Shared.Models.V1.Inventory.Warehouse.Responses;

namespace Energy.Infrastructure.Inventory.Warehouse.Lookups;

/// <summary>Warehouse lookup servisi (aktif + arama filtreli projection).</summary>
public class WarehouseLookupService : IWarehouseLookupService
{
    private readonly AppDbContext _db;

    public WarehouseLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WarehouseLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.Warehouses.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new WarehouseLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<WarehouseLookupResponse>>.Success(items);
    }
}
