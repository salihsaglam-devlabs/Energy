using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Operations.WorkOrderMaterialPlan.Lookups;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Responses;

namespace Energy.Infrastructure.Modules.Operations.WorkOrderMaterialPlan.Lookups;

/// <summary>WorkOrderMaterialPlan lookup servisi (aktif + arama filtreli projection).</summary>
public class WorkOrderMaterialPlanLookupService : IWorkOrderMaterialPlanLookupService
{
    private readonly EnergyDbContext _db;

    public WorkOrderMaterialPlanLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WorkOrderMaterialPlanLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.WorkOrderMaterialPlans.AsNoTracking();
        var items = await query.Select(e => new WorkOrderMaterialPlanLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<WorkOrderMaterialPlanLookupResponse>>.Success(items);
    }
}
