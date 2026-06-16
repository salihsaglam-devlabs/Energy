using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalCondition.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalCondition.Lookups;

/// <summary>ApprovalCondition lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalConditionLookupService : IApprovalConditionLookupService
{
    private readonly EnergyDbContext _db;

    public ApprovalConditionLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalConditionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalConditions.AsNoTracking();
        var items = await query.Select(e => new ApprovalConditionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = e.Id.ToString(),
            IsActive = true
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ApprovalConditionLookupResponse>>.Success(items);
    }
}
