using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.IAM.User.Requests;
using Energy.Shared.Models.V1.IAM.User.Responses;

namespace Energy.Application.IAM.User.Services;

/// <summary>User CRUD use-case sözleşmesi.</summary>
public interface IUserService
{
    /// <summary>Sayfalanmış User listesi.</summary>
    Task<BaseResponse<PaginatedResponse<UserListResponse>>> GetListAsync(GetUserListRequest request, CancellationToken ct = default);

    /// <summary>Kimliğe göre detay.</summary>
    Task<BaseResponse<UserDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Yeni kayıt oluşturur; yeni kimliği döndürür.</summary>
    Task<BaseResponse<Guid>> CreateAsync(CreateUserRequest request, CancellationToken ct = default);

    /// <summary>Var olan kaydı günceller.</summary>
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken ct = default);

    /// <summary>Kaydı (gerekiyorsa soft-delete) siler.</summary>
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
