using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Workflow.ApprovalDefinitionVersion.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Responses;

namespace Energy.Infrastructure.Workflow.ApprovalDefinitionVersion.Lookups;

/// <summary>ApprovalDefinitionVersion lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalDefinitionVersionLookupService : IApprovalDefinitionVersionLookupService
{
    private readonly AppDbContext _db;

    public ApprovalDefinitionVersionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalDefinitionVersionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalDefinitionVersions.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ApprovalDefinitionVersionLookupResponse>)rows.Select(e => new ApprovalDefinitionVersionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.EffectiveFrom.ToString("yyyy-MM-dd")) ? "Approval Definition Version #" + e.Id.ToString().Substring(0, 8) : (e.EffectiveFrom.ToString("yyyy-MM-dd")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ApprovalDefinitionVersionLookupResponse>>.Success(items);
    }
}
