using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalDefinitionVersion.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalDefinitionVersion.Lookups;

/// <summary>ApprovalDefinitionVersion lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalDefinitionVersionLookupService : IApprovalDefinitionVersionLookupService
{
    private readonly AppDbContext _db;

    public ApprovalDefinitionVersionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalDefinitionVersionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalDefinitionVersions.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        var items = await query
            .OrderBy(e => e.Id)
            .Select(e => new ApprovalDefinitionVersionLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = null,
                DisplayName = e.Id.ToString(),
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ApprovalDefinitionVersionLookupResponse>>.Success(items);
    }
}
