using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalDefinition.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalDefinition.Lookups;

/// <summary>ApprovalDefinition lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalDefinitionLookupService : IApprovalDefinitionLookupService
{
    private readonly EnergyDbContext _db;

    public ApprovalDefinitionLookupService(EnergyDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalDefinitionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalDefinitions.AsNoTracking();
        if (activeOnly) query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query.Select(e => new ApprovalDefinitionLookupResponse
        {
            Id = e.Id,
            Code = e.Code,
            Name = e.Name,
            DisplayName = e.Name,
            IsActive = e.IsActive
        }).ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ApprovalDefinitionLookupResponse>>.Success(items);
    }
}
