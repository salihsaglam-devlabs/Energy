using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Responses;

namespace Energy.Application.Workflow.ApprovalDefinition.Lookups;

/// <summary>ApprovalDefinition lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IApprovalDefinitionLookupService
{
    /// <summary>ApprovalDefinition lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ApprovalDefinitionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
