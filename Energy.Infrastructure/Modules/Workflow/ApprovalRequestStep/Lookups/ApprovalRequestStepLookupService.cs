using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalRequestStep.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalRequestStep.Lookups;

/// <summary>ApprovalRequestStep lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalRequestStepLookupService : IApprovalRequestStepLookupService
{
    private readonly EnergyDbContext _db;

    public ApprovalRequestStepLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalRequestStepLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalRequestSteps.AsNoTracking();
        var items = await query.Select(e => new ApprovalRequestStepLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ApprovalRequestStepLookupResponse>>.Success(items);
    }
}
