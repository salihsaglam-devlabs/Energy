using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Workflow.ApprovalDefinition.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Responses;

namespace Energy.Infrastructure.Workflow.ApprovalDefinition.Lookups;

/// <summary>ApprovalDefinition lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalDefinitionLookupService : IApprovalDefinitionLookupService
{
    private readonly AppDbContext _db;

    public ApprovalDefinitionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalDefinitionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalDefinitions.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search) || e.Code.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new ApprovalDefinitionLookupResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                DisplayName = e.Code + " - " + e.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ApprovalDefinitionLookupResponse>>.Success(items);
    }
}
