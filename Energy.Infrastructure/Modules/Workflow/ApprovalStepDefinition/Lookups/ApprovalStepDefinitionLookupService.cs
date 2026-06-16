using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Modules.Workflow.ApprovalStepDefinition.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Responses;

namespace Energy.Infrastructure.Modules.Workflow.ApprovalStepDefinition.Lookups;

/// <summary>ApprovalStepDefinition lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalStepDefinitionLookupService : IApprovalStepDefinitionLookupService
{
    private readonly AppDbContext _db;

    public ApprovalStepDefinitionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalStepDefinitionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalStepDefinitions.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(e => e.Name.Contains(search));
        var items = await query
            .OrderBy(e => e.Name)
            .Select(e => new ApprovalStepDefinitionLookupResponse
            {
                Id = e.Id,
                Code = null,
                Name = e.Name,
                DisplayName = e.Name,
                IsActive = true
            })
            .ToListAsync(ct);
        return BaseResponse<IReadOnlyList<ApprovalStepDefinitionLookupResponse>>.Success(items);
    }
}
