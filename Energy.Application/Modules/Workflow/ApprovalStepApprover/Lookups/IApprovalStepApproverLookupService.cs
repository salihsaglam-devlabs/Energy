using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Responses;

namespace Energy.Application.Modules.Workflow.ApprovalStepApprover.Lookups;

/// <summary>ApprovalStepApprover lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IApprovalStepApproverLookupService
{
    /// <summary>ApprovalStepApprover lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ApprovalStepApproverLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
