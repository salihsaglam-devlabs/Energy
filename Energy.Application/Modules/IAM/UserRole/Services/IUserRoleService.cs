using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.UserRole.Requests;
using Energy.Shared.Models.V1.IAM.UserRole.Responses;

namespace Energy.Application.Modules.IAM.UserRole.Services;

/// <summary>UserRole CRUD use-case sözleşmesi.</summary>
public interface IUserRoleService
{
    /// <summary>Sayfalanmış UserRole listesi.</summary>
    Task<BaseResponse<PaginatedResponse<UserRoleListResponse>>> GetListAsync(GetUserRoleListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<UserRoleDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateUserRoleRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUserRoleRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
