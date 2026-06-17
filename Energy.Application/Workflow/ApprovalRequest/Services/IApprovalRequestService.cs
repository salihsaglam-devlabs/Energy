using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Responses;

namespace Energy.Application.Workflow.ApprovalRequest.Services;

/// <summary>ApprovalRequest CRUD use-case sözleşmesi.</summary>
public interface IApprovalRequestService
{
    /// <summary>Sayfalanmış ApprovalRequest listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ApprovalRequestListResponse>>> GetListAsync(GetApprovalRequestListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ApprovalRequestDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateApprovalRequestRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalRequestRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
