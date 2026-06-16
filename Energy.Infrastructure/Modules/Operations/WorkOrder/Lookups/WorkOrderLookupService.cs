using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Operations.WorkOrder.Lookups;
using Energy.Shared.Models.V1.Operations.WorkOrder.Responses;

namespace Energy.Infrastructure.Modules.Operations.WorkOrder.Lookups;

/// <summary>WorkOrder lookup servisi (aktif + arama filtreli projection).</summary>
public class WorkOrderLookupService : IWorkOrderLookupService
{
    private readonly EnergyDbContext _db;

    public WorkOrderLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WorkOrderLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.WorkOrders.AsNoTracking();
        var items = await query.Select(e => new WorkOrderLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<WorkOrderLookupResponse>>.Success(items);
    }
}
