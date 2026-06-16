using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Responses;

namespace Energy.Application.Modules.Workflow.ApprovalRequestApprover.Services;

/// <summary>ApprovalRequestApprover CRUD use-case sözleşmesi.</summary>
public interface IApprovalRequestApproverService
{
    /// <summary>Sayfalanmış ApprovalRequestApprover listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ApprovalRequestApproverListResponse>>> GetListAsync(GetApprovalRequestApproverListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ApprovalRequestApproverDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateApprovalRequestApproverRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalRequestApproverRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
