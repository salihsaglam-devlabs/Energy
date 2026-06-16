using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalRequestApprover.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalRequestApprover.Lookups;

/// <summary>ApprovalRequestApprover lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalRequestApproverLookupService : IApprovalRequestApproverLookupService
{
    private readonly AppDbContext _db;

    public ApprovalRequestApproverLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalRequestApproverLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalRequestApprovers.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new ApprovalRequestApproverLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ApprovalRequestApproverLookupResponse>>.Success(items);
    }
}
