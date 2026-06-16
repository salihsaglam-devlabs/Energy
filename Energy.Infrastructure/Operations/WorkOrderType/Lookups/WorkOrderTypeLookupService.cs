using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Operations.WorkOrderType.Lookups;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Responses;

namespace Energy.Infrastructure.Operations.WorkOrderType.Lookups;

/// <summary>WorkOrderType lookup servisi (aktif + arama filtreli projection).</summary>
public class WorkOrderTypeLookupService : IWorkOrderTypeLookupService
{
    private readonly AppDbContext _db;

    public WorkOrderTypeLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WorkOrderTypeLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.WorkOrderTypes.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new WorkOrderTypeLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<WorkOrderTypeLookupResponse>>.Success(items);
    }
}
