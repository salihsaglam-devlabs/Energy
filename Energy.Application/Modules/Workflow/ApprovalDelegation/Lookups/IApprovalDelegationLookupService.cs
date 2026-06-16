using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Responses;

namespace Energy.Application.Modules.Workflow.ApprovalDelegation.Lookups;

/// <summary>ApprovalDelegation lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IApprovalDelegationLookupService
{
    /// <summary>ApprovalDelegation lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ApprovalDelegationLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
