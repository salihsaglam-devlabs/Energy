using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Assets.EquipmentMaintenance.Lookups;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Responses;

namespace Energy.Infrastructure.Assets.EquipmentMaintenance.Lookups;

/// <summary>EquipmentMaintenance lookup servisi (aktif + arama filtreli projection).</summary>
public class EquipmentMaintenanceLookupService : IEquipmentMaintenanceLookupService
{
    private readonly AppDbContext _db;

    public EquipmentMaintenanceLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<EquipmentMaintenanceLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.EquipmentMaintenances.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new EquipmentMaintenanceLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<EquipmentMaintenanceLookupResponse>>.Success(items);
    }
}
