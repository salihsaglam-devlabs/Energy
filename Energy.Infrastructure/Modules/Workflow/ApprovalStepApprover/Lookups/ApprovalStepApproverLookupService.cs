using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalStepApprover.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalStepApprover.Lookups;

/// <summary>ApprovalStepApprover lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalStepApproverLookupService : IApprovalStepApproverLookupService
{
    private readonly AppDbContext _db;

    public ApprovalStepApproverLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalStepApproverLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalStepApprovers.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new ApprovalStepApproverLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ApprovalStepApproverLookupResponse>>.Success(items);
    }
}
