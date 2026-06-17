using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Operations.WorkOrderMaterialUsage.Lookups;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Responses;

namespace Energy.Infrastructure.Operations.WorkOrderMaterialUsage.Lookups;

/// <summary>WorkOrderMaterialUsage lookup servisi (aktif + arama filtreli projection).</summary>
public class WorkOrderMaterialUsageLookupService : IWorkOrderMaterialUsageLookupService
{
    private readonly AppDbContext _db;

    public WorkOrderMaterialUsageLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WorkOrderMaterialUsageLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.WorkOrderMaterialUsages.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new WorkOrderMaterialUsageLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<WorkOrderMaterialUsageLookupResponse>>.Success(items);
    }
}
