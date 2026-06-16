using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Assets.EquipmentAsset.Lookups;
using Energy.Shared.Models.V1.Assets.EquipmentAsset.Responses;

namespace Energy.Infrastructure.Modules.Assets.EquipmentAsset.Lookups;

/// <summary>EquipmentAsset lookup servisi (aktif + arama filtreli projection).</summary>
public class EquipmentAssetLookupService : IEquipmentAssetLookupService
{
    private readonly AppDbContext _db;

    public EquipmentAssetLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<EquipmentAssetLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.EquipmentAssets.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new EquipmentAssetLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<EquipmentAssetLookupResponse>>.Success(items);
    }
}
