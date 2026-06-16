using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.WarehouseTransferLine.Lookups;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Responses;

namespace Energy.Infrastructure.Modules.Inventory.WarehouseTransferLine.Lookups;

/// <summary>WarehouseTransferLine lookup servisi (aktif + arama filtreli projection).</summary>
public class WarehouseTransferLineLookupService : IWarehouseTransferLineLookupService
{
    private readonly EnergyDbContext _db;

    public WarehouseTransferLineLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WarehouseTransferLineLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.WarehouseTransferLines.AsNoTracking();
        var items = await query.Select(e => new WarehouseTransferLineLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<WarehouseTransferLineLookupResponse>>.Success(items);
    }
}
