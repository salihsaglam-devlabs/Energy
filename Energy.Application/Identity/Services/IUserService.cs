using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Application.Identity.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserSummaryResponse>> GetUsersAsync(CancellationToken cancellationToken = default);

    Task<UserDetailResponse> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<UserDetailResponse> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);

    Task<UserDetailResponse> UpdateUserAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default);

    Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);

    Task<UserDetailResponse> SetUserRolesAsync(Guid id, IReadOnlyList<Guid> roleIds, CancellationToken cancellationToken = default);

    Task UpdatePasswordAsync(Guid id, string newPassword, CancellationToken cancellationToken = default);

    Task<CredentialValidationResponse> ValidateCredentialsAsync(ValidateCredentialsRequest request, CancellationToken cancellationToken = default);

    Task<SeedAdminResponse> SeedAdminAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<AdminPermissionHealthResponse> GetAdminPermissionHealthAsync(CancellationToken cancellationToken = default);

    Task<ProfileImageResponse?> GetProfileImageAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<UserDetailResponse> SetProfileImageAsync(Guid userId, byte[] content, string contentType, CancellationToken cancellationToken = default);

    Task<UserDetailResponse> RemoveProfileImageAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resolves the database id of the currently authenticated user, trying the
    /// id claim first and falling back to email / user name lookups so the call
    /// stays robust against cookie/JWT drift after a database reseed.
    /// Returns <c>null</c> when no matching user row can be located.
    /// </summary>
    Task<Guid?> ResolveCurrentUserIdAsync(
        Guid? claimUserId,
        string? email,
        string? userName,
        CancellationToken cancellationToken = default);
}
