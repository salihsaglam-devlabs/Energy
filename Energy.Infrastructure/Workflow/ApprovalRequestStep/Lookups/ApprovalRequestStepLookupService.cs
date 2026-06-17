using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Workflow.ApprovalRequestStep.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;

namespace Energy.Infrastructure.Workflow.ApprovalRequestStep.Lookups;

/// <summary>ApprovalRequestStep lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalRequestStepLookupService : IApprovalRequestStepLookupService
{
    private readonly AppDbContext _db;

    public ApprovalRequestStepLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalRequestStepLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalRequestSteps.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ApprovalRequestStepLookupResponse>)rows.Select(e => new ApprovalRequestStepLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.Status.ToString()) ? "Approval Request Step #" + e.Id.ToString().Substring(0, 8) : (e.Status.ToString()),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ApprovalRequestStepLookupResponse>>.Success(items);
    }
}
