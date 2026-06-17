using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Assets.EquipmentAssignment.Lookups;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Responses;

namespace Energy.Infrastructure.Assets.EquipmentAssignment.Lookups;

/// <summary>EquipmentAssignment lookup servisi (aktif + arama filtreli projection).</summary>
public class EquipmentAssignmentLookupService : IEquipmentAssignmentLookupService
{
    private readonly AppDbContext _db;

    public EquipmentAssignmentLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<EquipmentAssignmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.EquipmentAssignments.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<EquipmentAssignmentLookupResponse>)rows.Select(e => new EquipmentAssignmentLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.StartDate.ToString("yyyy-MM-dd")) ? "Equipment Assignment #" + e.Id.ToString().Substring(0, 8) : (e.StartDate.ToString("yyyy-MM-dd")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<EquipmentAssignmentLookupResponse>>.Success(items);
    }
}
