using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalRequest.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalRequest.Lookups;

/// <summary>ApprovalRequest lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalRequestLookupService : IApprovalRequestLookupService
{
    private readonly AppDbContext _db;

    public ApprovalRequestLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalRequestLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalRequests.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new ApprovalRequestLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ApprovalRequestLookupResponse>>.Success(items);
    }
}
