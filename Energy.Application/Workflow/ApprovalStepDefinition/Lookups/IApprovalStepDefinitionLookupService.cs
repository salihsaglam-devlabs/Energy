using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Responses;

namespace Energy.Application.Workflow.ApprovalStepDefinition.Lookups;

/// <summary>ApprovalStepDefinition lookup sözleşmesi (aktif kayıt + arama filtreli).</summary>
public interface IApprovalStepDefinitionLookupService
{
    /// <summary>ApprovalStepDefinition lookup listesi döndürür.</summary>
    Task<BaseResponse<IReadOnlyList<ApprovalStepDefinitionLookupResponse>>> GetLookupAsync(string? search = null, bool activeOnly = true, CancellationToken ct = default);
}
