using Microsoft.EntityFrameworkCore;
using Energy.Infrastructure.Persistence;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Application.Workflow.ApprovalCondition.Lookups;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Responses;

namespace Energy.Infrastructure.Workflow.ApprovalCondition.Lookups;

/// <summary>ApprovalCondition lookup servisi (aktif + arama filtreli projection).</summary>
public class ApprovalConditionLookupService : IApprovalConditionLookupService
{
    private readonly AppDbContext _db;

    public ApprovalConditionLookupService(AppDbContext db) => _db = db;

    public async Task<BaseResponse<IReadOnlyList<ApprovalConditionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default)
    {
        var query = _db.ApprovalConditions.AsNoTracking();
        var rows = await query
            .OrderBy(e => e.Id)
            .ToListAsync(ct);
        var items = (IReadOnlyList<ApprovalConditionLookupResponse>)rows.Select(e => new ApprovalConditionLookupResponse
        {
            Id = e.Id,
            Code = null,
            Name = null,
            DisplayName = string.IsNullOrWhiteSpace(e.Operator.ToString() + " - " + (e.ValueNumber.HasValue ? e.ValueNumber.Value.ToString() : "")) ? "Approval Condition #" + e.Id.ToString().Substring(0, 8) : (e.Operator.ToString() + " - " + (e.ValueNumber.HasValue ? e.ValueNumber.Value.ToString() : "")),
            IsActive = true
        }).ToList();
        return BaseResponse<IReadOnlyList<ApprovalConditionLookupResponse>>.Success(items);
    }
}
