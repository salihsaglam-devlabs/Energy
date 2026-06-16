using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Responses;

namespace Energy.Application.Modules.Workflow.ApprovalRequest.Lookups;

/// <summary>ApprovalRequest lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IApprovalRequestLookupService
{
    /// <summary>ApprovalRequest lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ApprovalRequestLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
