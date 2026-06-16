using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.UserPermission.Requests;
using Energy.Shared.Models.V1.IAM.UserPermission.Responses;

namespace Energy.Application.Modules.IAM.UserPermission.Services;

/// <summary>UserPermission CRUD use-case sözleşmesi.</summary>
public interface IUserPermissionService
{
    /// <summary>Sayfalanmış UserPermission listesi.</summary>
    Task<BaseResponse<PaginatedResponse<UserPermissionListResponse>>> GetListAsync(GetUserPermissionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<UserPermissionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateUserPermissionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUserPermissionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
