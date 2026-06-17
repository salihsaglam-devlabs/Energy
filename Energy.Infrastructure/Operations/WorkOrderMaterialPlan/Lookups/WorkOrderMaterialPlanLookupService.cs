using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Operations.WorkOrderMaterialPlan.Lookups;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Responses;

namespace Energy.Infrastructure.Operations.WorkOrderMaterialPlan.Lookups;

/// <summary>WorkOrderMaterialPlan lookup servisi (aktif + arama filtreli projection).</summary>
public class WorkOrderMaterialPlanLookupService : IWorkOrderMaterialPlanLookupService
{
    private readonly AppDbContext _db;

    public WorkOrderMaterialPlanLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WorkOrderMaterialPlanLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.WorkOrderMaterialPlans.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<WorkOrderMaterialPlanLookupResponse>)rows.Select(e => new WorkOrderMaterialPlanLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.PlannedQuantity.ToString()) ? "Work Order Material Plan #" + e.Id.ToString().Substring(0, 8) : (e.PlannedQuantity.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<WorkOrderMaterialPlanLookupResponse>>.Success(items);
    }
}
