using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Responses;

namespace Energy.Application.Modules.Workflow.ApprovalStepApprover.Services;

/// <summary>ApprovalStepApprover CRUD use-case sözleşmesi.</summary>
public interface IApprovalStepApproverService
{
    /// <summary>Sayfalanmış ApprovalStepApprover listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ApprovalStepApproverListResponse>>> GetListAsync(GetApprovalStepApproverListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ApprovalStepApproverDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateApprovalStepApproverRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalStepApproverRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
