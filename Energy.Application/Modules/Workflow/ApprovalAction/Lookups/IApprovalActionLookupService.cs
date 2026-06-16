using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Responses;

namespace Energy.Application.Modules.Workflow.ApprovalAction.Lookups;

/// <summary>ApprovalAction lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IApprovalActionLookupService
{
    /// <summary>ApprovalAction lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ApprovalActionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
