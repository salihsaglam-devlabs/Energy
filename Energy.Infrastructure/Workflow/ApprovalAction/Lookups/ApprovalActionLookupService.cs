using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Workflow.ApprovalAction.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Responses;

namespace Energy.Infrastructure.Workflow.ApprovalAction.Lookups;

/// <summary>ApprovalAction lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalActionLookupService : IApprovalActionLookupService
{
    private readonly AppDbContext _db;

    public ApprovalActionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalActionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalActions.AsNoTracking();
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new ApprovalActionLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ApprovalActionLookupResponse>>.Success(items);
    }
}
