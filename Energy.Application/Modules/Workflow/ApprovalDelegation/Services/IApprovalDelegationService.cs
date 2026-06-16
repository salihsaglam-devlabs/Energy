using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Responses;

namespace Energy.Application.Modules.Workflow.ApprovalDelegation.Services;

/// <summary>ApprovalDelegation CRUD use-case sözleşmesi.</summary>
public interface IApprovalDelegationService
{
    /// <summary>Sayfalanmış ApprovalDelegation listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ApprovalDelegationListResponse>>> GetListAsync(GetApprovalDelegationListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ApprovalDelegationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateApprovalDelegationRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalDelegationRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
