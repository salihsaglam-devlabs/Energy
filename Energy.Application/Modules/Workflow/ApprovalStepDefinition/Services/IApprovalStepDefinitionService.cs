using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Responses;

namespace Energy.Application.Modules.Workflow.ApprovalStepDefinition.Services;

/// <summary>ApprovalStepDefinition CRUD use-case sözleşmesi.</summary>
public interface IApprovalStepDefinitionService
{
    /// <summary>Sayfalanmış ApprovalStepDefinition listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ApprovalStepDefinitionListResponse>>> GetListAsync(GetApprovalStepDefinitionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ApprovalStepDefinitionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateApprovalStepDefinitionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalStepDefinitionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
