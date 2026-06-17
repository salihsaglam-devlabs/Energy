using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Responses;

namespace Energy.Application.Workflow.ApprovalAction.Services;

/// <summary>ApprovalAction CRUD use-case sözleşmesi.</summary>
public interface IApprovalActionService
{
    /// <summary>Sayfalanmış ApprovalAction listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ApprovalActionListResponse>>> GetListAsync(GetApprovalActionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ApprovalActionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateApprovalActionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalActionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
