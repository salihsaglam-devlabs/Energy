using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.Permission.Requests;
using Energy.Shared.Models.V1.IAM.Permission.Responses;

namespace Energy.Application.Modules.IAM.Permission.Services;

/// <summary>Permission CRUD use-case sözleşmesi.</summary>
public interface IPermissionService
{
    /// <summary>Sayfalanmış Permission listesi.</summary>
    Task<BaseResponse<PaginatedResponse<PermissionListResponse>>> GetListAsync(GetPermissionListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<PermissionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreatePermissionRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePermissionRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
