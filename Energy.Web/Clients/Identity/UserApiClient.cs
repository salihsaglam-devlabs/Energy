using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Identity;

public sealed class UserApiClient : ApiClientBase, IUserApiClient
{
    public UserApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<BaseResponse<PaginatedResponse<UserSummaryResponse>>> GetUsersAsync(PaginatedRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<PaginatedResponse<UserSummaryResponse>>>(ApiQueryString.Append(ApiRoutes.Users.Base, request), cancellationToken);

    public Task<BaseResponse<UserDetailResponse>> GetUserAsync(Guid id, CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<UserDetailResponse>>(ApiRoutes.Users.ById(id), cancellationToken);

    public Task<BaseResponse<UserDetailResponse>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
        => PostAsync<CreateUserRequest, BaseResponse<UserDetailResponse>>(ApiRoutes.Users.Base, request, cancellationToken);

    public Task<BaseResponse<UserDetailResponse>> UpdateUserAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default)
        => PutAsync<UpdateUserRequest, BaseResponse<UserDetailResponse>>(ApiRoutes.Users.ById(id), request, cancellationToken);

    public Task<BaseResponse<UserDetailResponse>> SetRolesAsync(Guid id, SetUserRolesRequest request, CancellationToken cancellationToken = default)
        => PutAsync<SetUserRolesRequest, BaseResponse<UserDetailResponse>>(ApiRoutes.Users.Roles(id), request, cancellationToken);

    public Task<BaseResponse<Guid>> UpdatePasswordAsync(Guid id, UpdateUserPasswordRequest request, CancellationToken cancellationToken = default)
        => PutAsync<UpdateUserPasswordRequest, BaseResponse<Guid>>(ApiRoutes.Users.Password(id), request, cancellationToken);

    public Task<BaseResponse<Guid>> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
        => DeleteAsync<BaseResponse<Guid>>(ApiRoutes.Users.ById(id), cancellationToken);

    public Task<BaseResponse<SeedAdminResponse>> SeedAdminAsync(CancellationToken cancellationToken = default)
        => PostAsync<BaseResponse<SeedAdminResponse>>(ApiRoutes.Users.SeedAdmin, cancellationToken);

    public Task<BaseResponse<UserDetailResponse>> GetCurrentUserAsync(CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<UserDetailResponse>>(ApiRoutes.Users.Me, cancellationToken);

    public async Task<(byte[] Content, string ContentType)?> GetProfileImageAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var (content, contentType, statusCode) = await GetRawAsync(ApiRoutes.Users.ProfileImage(id), cancellationToken);
        if (statusCode == 404 || content.Length == 0)
        {
            return null;
        }

        return (content, contentType);
    }

    public Task<BaseResponse<UserDetailResponse>> UpdateProfileImageAsync(Guid id, UpdateProfileImageRequest request, CancellationToken cancellationToken = default)
        => PutAsync<UpdateProfileImageRequest, BaseResponse<UserDetailResponse>>(ApiRoutes.Users.ProfileImage(id), request, cancellationToken);

    public Task<BaseResponse<UserDetailResponse>> RemoveProfileImageAsync(Guid id, CancellationToken cancellationToken = default)
        => DeleteAsync<BaseResponse<UserDetailResponse>>(ApiRoutes.Users.ProfileImage(id), cancellationToken);

    public Task<BaseResponse<UserDetailResponse>> UpdateMyProfileImageAsync(UpdateProfileImageRequest request, CancellationToken cancellationToken = default)
        => PutAsync<UpdateProfileImageRequest, BaseResponse<UserDetailResponse>>(ApiRoutes.Users.MyProfileImage, request, cancellationToken);

    public Task<BaseResponse<UserDetailResponse>> RemoveMyProfileImageAsync(CancellationToken cancellationToken = default)
        => DeleteAsync<BaseResponse<UserDetailResponse>>(ApiRoutes.Users.MyProfileImage, cancellationToken);
}
