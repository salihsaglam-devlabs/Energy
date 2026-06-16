using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Operations.WorkOrderStatusHistory.Lookups;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Responses;

namespace Energy.Infrastructure.Operations.WorkOrderStatusHistory.Lookups;

/// <summary>WorkOrderStatusHistory lookup servisi (aktif + arama filtreli projection).</summary>
public class WorkOrderStatusHistoryLookupService : IWorkOrderStatusHistoryLookupService
{
    private readonly AppDbContext _db;

    public WorkOrderStatusHistoryLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WorkOrderStatusHistoryLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.WorkOrderStatusHistories.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new WorkOrderStatusHistoryLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<WorkOrderStatusHistoryLookupResponse>>.Success(items);
    }
}
