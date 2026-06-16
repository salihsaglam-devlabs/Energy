using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;

namespace Energy.Application.Workflow.ApprovalRequestStep.Lookups;

/// <summary>ApprovalRequestStep lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IApprovalRequestStepLookupService
{
    /// <summary>ApprovalRequestStep lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ApprovalRequestStepLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
