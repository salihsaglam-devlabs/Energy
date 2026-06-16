using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.WarehouseLocation.Lookups;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Responses;

namespace Energy.Infrastructure.Modules.Inventory.WarehouseLocation.Lookups;

/// <summary>WarehouseLocation lookup servisi (aktif + arama filtreli projection).</summary>
public class WarehouseLocationLookupService : IWarehouseLocationLookupService
{
    private readonly AppDbContext _db;

    public WarehouseLocationLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WarehouseLocationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.WarehouseLocations.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new WarehouseLocationLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<WarehouseLocationLookupResponse>>.Success(items);
    }
}
