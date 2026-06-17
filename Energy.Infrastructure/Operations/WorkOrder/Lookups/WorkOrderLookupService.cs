using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Operations.WorkOrder.Lookups;
using Energy.Shared.Models.V1.Operations.WorkOrder.Responses;

namespace Energy.Infrastructure.Operations.WorkOrder.Lookups;

/// <summary>WorkOrder lookup servisi (aktif + arama filtreli projection).</summary>
public class WorkOrderLookupService : IWorkOrderLookupService
{
    private readonly AppDbContext _db;

    public WorkOrderLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<WorkOrderLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.WorkOrders.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Title.Contains(search));
        var items = await query
            .OrderBy(e => e.Title)
            .Select(e => new WorkOrderLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.Title,
                DisplayName = e.Title,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<WorkOrderLookupResponse>>.Success(items);
    }
}
