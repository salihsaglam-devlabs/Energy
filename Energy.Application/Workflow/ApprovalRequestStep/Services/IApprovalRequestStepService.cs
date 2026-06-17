using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Responses;

namespace Energy.Application.Workflow.ApprovalRequestStep.Services;

/// <summary>ApprovalRequestStep CRUD use-case sözleşmesi.</summary>
public interface IApprovalRequestStepService
{
    /// <summary>Sayfalanmış ApprovalRequestStep listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ApprovalRequestStepListResponse>>> GetListAsync(GetApprovalRequestStepListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ApprovalRequestStepDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateApprovalRequestStepRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalRequestStepRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
