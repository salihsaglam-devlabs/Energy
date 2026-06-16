using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Requests;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Responses;

namespace Energy.Application.Modules.Workflow.ApprovalDefinitionVersion.Services;

/// <summary>ApprovalDefinitionVersion CRUD use-case sözleşmesi.</summary>
public interface IApprovalDefinitionVersionService
{
    /// <summary>Sayfalanmış ApprovalDefinitionVersion listesi.</summary>
    Task<BaseResponse<PaginatedResponse<ApprovalDefinitionVersionListResponse>>> GetListAsync(GetApprovalDefinitionVersionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<ApprovalDefinitionVersionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateApprovalDefinitionVersionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateApprovalDefinitionVersionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
