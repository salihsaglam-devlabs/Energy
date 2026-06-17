using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Operations.WorkOrderAssignment.Lookups;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;

namespace Energy.Infrastructure.Operations.WorkOrderAssignment.Lookups;

/// <summary>WorkOrderAssignment lookup servisi (aktif + arama filtreli projection).</summary>
public class WorkOrderAssignmentLookupService : IWorkOrderAssignmentLookupService
{
    private readonly AppDbContext _db;

    public WorkOrderAssignmentLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WorkOrderAssignmentLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.WorkOrderAssignments.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<WorkOrderAssignmentLookupResponse>)rows.Select(e => new WorkOrderAssignmentLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.AssignmentRole ?? "")) ? "Work Order Assignment #" + e.Id.ToString().Substring(0, 8) : ((e.AssignmentRole ?? "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<WorkOrderAssignmentLookupResponse>>.Success(items);
    }
}
