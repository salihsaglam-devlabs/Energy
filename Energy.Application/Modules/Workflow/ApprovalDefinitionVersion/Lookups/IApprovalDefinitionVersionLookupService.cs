using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Responses;

namespace Energy.Application.Modules.Workflow.ApprovalDefinitionVersion.Lookups;

/// <summary>ApprovalDefinitionVersion lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IApprovalDefinitionVersionLookupService
{
    /// <summary>ApprovalDefinitionVersion lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ApprovalDefinitionVersionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
