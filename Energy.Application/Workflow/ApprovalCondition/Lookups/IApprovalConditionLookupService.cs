using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Responses;

namespace Energy.Application.Workflow.ApprovalCondition.Lookups;

/// <summary>ApprovalCondition lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IApprovalConditionLookupService
{
    /// <summary>ApprovalCondition lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ApprovalConditionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
