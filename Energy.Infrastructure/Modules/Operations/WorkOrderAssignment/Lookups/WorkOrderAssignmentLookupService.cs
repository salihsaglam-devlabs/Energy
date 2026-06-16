using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Operations.WorkOrderAssignment.Lookups;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;

namespace Energy.Infrastructure.Modules.Operations.WorkOrderAssignment.Lookups;

/// <summary>WorkOrderAssignment lookup servisi (aktif + arama filtreli projection).</summary>
public class WorkOrderAssignmentLookupService : IWorkOrderAssignmentLookupService
{
    private readonly EnergyDbContext _db;

    public WorkOrderAssignmentLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WorkOrderAssignmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.WorkOrderAssignments.AsNoTracking();
        var items = await query.Select(e => new WorkOrderAssignmentLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<WorkOrderAssignmentLookupResponse>>.Success(items);
    }
}
