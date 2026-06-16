using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Responses;

namespace Energy.Application.Workflow.ApprovalCondition.Services;

/// <summary>ApprovalCondition CRUD use-case sözleşmesi.</summary>
public interface IApprovalConditionService
{
    /// <summary>Sayfalanmış ApprovalCondition listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ApprovalConditionListResponse>>> GetListAsync(GetApprovalConditionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ApprovalConditionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateApprovalConditionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalConditionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
