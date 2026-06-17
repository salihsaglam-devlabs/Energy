using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Workflow.ApprovalStepApprover.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Responses;

namespace Energy.Infrastructure.Workflow.ApprovalStepApprover.Lookups;

/// <summary>ApprovalStepApprover lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalStepApproverLookupService : IApprovalStepApproverLookupService
{
    private readonly AppDbContext _db;

    public ApprovalStepApproverLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalStepApproverLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalStepApprovers.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ApprovalStepApproverLookupResponse>)rows.Select(e => new ApprovalStepApproverLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.ApproverType.ToString()) ? "Approval Step Approver #" + e.Id.ToString().Substring(0, 8) : (e.ApproverType.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ApprovalStepApproverLookupResponse>>.Success(items);
    }
}
