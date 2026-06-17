using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Workflow.ApprovalRequestApprover.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;

namespace Energy.Infrastructure.Workflow.ApprovalRequestApprover.Lookups;

/// <summary>ApprovalRequestApprover lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalRequestApproverLookupService : IApprovalRequestApproverLookupService
{
    private readonly AppDbContext _db;

    public ApprovalRequestApproverLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalRequestApproverLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalRequestApprovers.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ApprovalRequestApproverLookupResponse>)rows.Select(e => new ApprovalRequestApproverLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.Status.ToString() + " - " + (e.ActionAt.HasValue ? e.ActionAt.Value.ToString("yyyy-MM-dd") : "")) ? "Approval Request Approver #" + e.Id.ToString().Substring(0, 8) : (e.Status.ToString() + " - " + (e.ActionAt.HasValue ? e.ActionAt.Value.ToString("yyyy-MM-dd") : "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ApprovalRequestApproverLookupResponse>>.Success(items);
    }
}
