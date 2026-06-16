using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Workflow.ApprovalDelegation.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Responses;

namespace Energy.Infrastructure.Workflow.ApprovalDelegation.Lookups;

/// <summary>ApprovalDelegation lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalDelegationLookupService : IApprovalDelegationLookupService
{
    private readonly AppDbContext _db;

    public ApprovalDelegationLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalDelegationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalDelegations.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new ApprovalDelegationLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ApprovalDelegationLookupResponse>>.Success(items);
    }
}
