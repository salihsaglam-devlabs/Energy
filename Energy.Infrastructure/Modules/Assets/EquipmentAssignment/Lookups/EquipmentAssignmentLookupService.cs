using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Assets.EquipmentAssignment.Lookups;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Responses;

namespace Energy.Infrastructure.Modules.Assets.EquipmentAssignment.Lookups;

/// <summary>EquipmentAssignment lookup servisi (aktif + arama filtreli projection).</summary>
public class EquipmentAssignmentLookupService : IEquipmentAssignmentLookupService
{
    private readonly AppDbContext _db;

    public EquipmentAssignmentLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<EquipmentAssignmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.EquipmentAssignments.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new EquipmentAssignmentLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<EquipmentAssignmentLookupResponse>>.Success(items);
    }
}
