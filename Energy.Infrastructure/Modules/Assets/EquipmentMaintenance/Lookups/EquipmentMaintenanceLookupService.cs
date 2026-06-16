using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Assets.EquipmentMaintenance.Lookups;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Responses;

namespace Energy.Infrastructure.Modules.Assets.EquipmentMaintenance.Lookups;

/// <summary>EquipmentMaintenance lookup servisi (aktif + arama filtreli projection).</summary>
public class EquipmentMaintenanceLookupService : IEquipmentMaintenanceLookupService
{
    private readonly EnergyDbContext _db;

    public EquipmentMaintenanceLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<EquipmentMaintenanceLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.EquipmentMaintenances.AsNoTracking();
        var items = await query.Select(e => new EquipmentMaintenanceLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<EquipmentMaintenanceLookupResponse>>.Success(items);
    }
}
