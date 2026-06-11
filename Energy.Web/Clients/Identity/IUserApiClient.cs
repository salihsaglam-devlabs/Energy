using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Web.Clients.Identity;

public interface IUserApiClient
{
    Task<BaseResponse<PaginatedResponse<UserSummaryResponse>>> GetUsersAsync(PaginatedRequest? request = null, CancellationToken cancellationToken = default);

    Task<BaseResponse<UserDetailResponse>> GetUserAsync(Guid id, CancellationToken cancellationToken = default);

    Task<BaseResponse<UserDetailResponse>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);

    Task<BaseResponse<UserDetailResponse>> UpdateUserAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default);

    Task<BaseResponse<UserDetailResponse>> SetRolesAsync(Guid id, SetUserRolesRequest request, CancellationToken cancellationToken = default);

    Task<BaseResponse<Guid>> UpdatePasswordAsync(Guid id, UpdateUserPasswordRequest request, CancellationToken cancellationToken = default);

    Task<BaseResponse<Guid>> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);

    Task<BaseResponse<SeedAdminResponse>> SeedAdminAsync(CancellationToken cancellationToken = default);

    Task<BaseResponse<UserDetailResponse>> GetCurrentUserAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads the raw profile image for the given user. Returns <c>null</c>
    /// when the API responds with 404 (no image set).
    /// </summary>
    Task<(byte[] Content, string ContentType)?> GetProfileImageAsync(Guid id, CancellationToken cancellationToken = default);

    Task<BaseResponse<UserDetailResponse>> UpdateProfileImageAsync(Guid id, UpdateProfileImageRequest request, CancellationToken cancellationToken = default);

    Task<BaseResponse<UserDetailResponse>> RemoveProfileImageAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>Updates the current caller's profile image (server resolves the id from the JWT).</summary>
    Task<BaseResponse<UserDetailResponse>> UpdateMyProfileImageAsync(UpdateProfileImageRequest request, CancellationToken cancellationToken = default);

    /// <summary>Removes the current caller's profile image (server resolves the id from the JWT).</summary>
    Task<BaseResponse<UserDetailResponse>> RemoveMyProfileImageAsync(CancellationToken cancellationToken = default);
}
