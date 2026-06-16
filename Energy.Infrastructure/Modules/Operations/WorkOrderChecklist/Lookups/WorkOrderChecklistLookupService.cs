using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Operations.WorkOrderChecklist.Lookups;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Responses;

namespace Energy.Infrastructure.Modules.Operations.WorkOrderChecklist.Lookups;

/// <summary>WorkOrderChecklist lookup servisi (aktif + arama filtreli projection).</summary>
public class WorkOrderChecklistLookupService : IWorkOrderChecklistLookupService
{
    private readonly AppDbContext _db;

    public WorkOrderChecklistLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WorkOrderChecklistLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.WorkOrderChecklists.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new WorkOrderChecklistLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.Name,
                DisplayName = e.Name,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<WorkOrderChecklistLookupResponse>>.Success(items);
    }
}
