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
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<WorkOrderChecklistItemLookupResponse>)rows.Select(e => new WorkOrderChecklistItemLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace((e.Description ?? "")) ? "Work Order Checklist Item #" + e.Id.ToString().Substring(0, 8) : ((e.Description ?? "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<WorkOrderChecklistItemLookupResponse>>.Success(items);
    }
}
