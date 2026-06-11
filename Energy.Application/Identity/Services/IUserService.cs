using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Application.Identity.Services;

public interface IUserService
{
    Task<PaginatedResponse<UserSummaryResponse>> GetAllAsync(PaginatedRequest request, CancellationToken cancellationToken = default);
    Task<UserDetailResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserDetailResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<UserDetailResponse> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ChangePasswordAsync(Guid id, ChangePasswordRequest request, CancellationToken cancellationToken = default);

    /// <summary>Returns the user's roles, role-inherited permissions and direct grants.</summary>
    Task<UserAccessResponse?> GetAccessAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Replaces the user's role assignments and direct permission grants in one operation.</summary>
    Task<UserAccessResponse> SetAccessAsync(Guid id, SetUserAccessRequest request, CancellationToken cancellationToken = default);
    Task<AuthTokenResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<ProfileImageResponse?> GetProfileImageAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Stores (or replaces) the user's profile image.</summary>
    Task<bool> SetProfileImageAsync(Guid id, byte[] content, string contentType, CancellationToken cancellationToken = default);

    /// <summary>Clears the user's profile image.</summary>
    Task<bool> RemoveProfileImageAsync(Guid id, CancellationToken cancellationToken = default);
}
