using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.Role.Requests;
using Energy.Shared.Models.V1.IAM.Role.Responses;

namespace Energy.Application.IAM.Role.Services;

/// <summary>Role CRUD use-case sözleşmesi.</summary>
public interface IRoleService
{
    /// <summary>Sayfalanmış Role listesi.</summary>
    Task<BaseResponse<PaginatedResponse<RoleListResponse>>> GetListAsync(GetRoleListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<RoleDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateRoleRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
