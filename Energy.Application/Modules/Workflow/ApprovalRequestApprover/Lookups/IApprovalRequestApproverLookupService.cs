using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;

namespace Energy.Application.Modules.Workflow.ApprovalRequestApprover.Lookups;

/// <summary>ApprovalRequestApprover lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IApprovalRequestApproverLookupService
{
    /// <summary>ApprovalRequestApprover lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ApprovalRequestApproverLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
