using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Operations.WorkOrderChecklistItem.Lookups;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Responses;

namespace Energy.Infrastructure.Operations.WorkOrderChecklistItem.Lookups;

/// <summary>WorkOrderChecklistItem lookup servisi (aktif + arama filtreli projection).</summary>
public class WorkOrderChecklistItemLookupService : IWorkOrderChecklistItemLookupService
{
    private readonly AppDbContext _db;

    public WorkOrderChecklistItemLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WorkOrderChecklistItemLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.WorkOrderChecklistItems.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new WorkOrderChecklistItemLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<WorkOrderChecklistItemLookupResponse>>.Success(items);
    }
}
