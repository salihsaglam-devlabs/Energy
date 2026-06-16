using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Inventory.WarehouseTransfer.Lookups;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Responses;

namespace Energy.Infrastructure.Modules.Inventory.WarehouseTransfer.Lookups;

/// <summary>WarehouseTransfer lookup servisi (aktif + arama filtreli projection).</summary>
public class WarehouseTransferLookupService : IWarehouseTransferLookupService
{
    private readonly AppDbContext _db;

    public WarehouseTransferLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WarehouseTransferLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.WarehouseTransfers.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.TransferNo.Contains(search));
        var items = await query
            .OrderBy(e => e.TransferNo)
            .Select(e => new WarehouseTransferLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.TransferNo,
                DisplayName = e.TransferNo,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<WarehouseTransferLookupResponse>>.Success(items);
    }
}
