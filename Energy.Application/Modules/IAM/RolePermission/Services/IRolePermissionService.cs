using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.RolePermission.Requests;
using Energy.Shared.Models.V1.IAM.RolePermission.Responses;

namespace Energy.Application.Modules.IAM.RolePermission.Services;

/// <summary>RolePermission CRUD use-case sözleşmesi.</summary>
public interface IRolePermissionService
{
    /// <summary>Sayfalanmış RolePermission listesi.</summary>
    Task<BaseResponse<PaginatedResponse<RolePermissionListResponse>>> GetListAsync(GetRolePermissionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<RolePermissionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateRolePermissionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateRolePermissionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
