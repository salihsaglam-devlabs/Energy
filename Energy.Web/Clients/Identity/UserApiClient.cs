using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Identity;

public sealed class UserApiClient : ApiClientBase, IUserApiClient
{
    public UserApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<UserSummaryResponse>>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<UserSummaryResponse>>>(
            $"{ApiRoutes.Users.Base}?pageNumber={request.PageNumber}&pageSize={request.PageSize}&search={Uri.EscapeDataString(request.Search ?? string.Empty)}", ct);

    public Task<BaseResponse<UserDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<UserDetailResponse>>(ApiRoutes.Users.ById(id), ct);

    public Task<BaseResponse<UserDetailResponse>> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
        => PostAsync<CreateUserRequest, BaseResponse<UserDetailResponse>>(ApiRoutes.Users.Base, request, ct);

    public Task<BaseResponse<UserDetailResponse>> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken ct = default)
        => PutAsync<UpdateUserRequest, BaseResponse<UserDetailResponse>>(ApiRoutes.Users.ById(id), request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>(ApiRoutes.Users.ById(id), ct);

    public Task<BaseResponse<bool>> ChangePasswordAsync(Guid id, ChangePasswordRequest request, CancellationToken ct = default)
        => PutAsync<ChangePasswordRequest, BaseResponse<bool>>(ApiRoutes.Users.Password(id), request, ct);

    public Task<BaseResponse<UserAccessResponse>> GetAccessAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<UserAccessResponse>>(ApiRoutes.Users.Access(id), ct);

    public Task<BaseResponse<UserAccessResponse>> SetAccessAsync(Guid id, SetUserAccessRequest request, CancellationToken ct = default)
        => PutAsync<SetUserAccessRequest, BaseResponse<UserAccessResponse>>(ApiRoutes.Users.Access(id), request, ct);

    public Task<(byte[] Content, string ContentType, int StatusCode)> GetProfileImageAsync(Guid id, CancellationToken ct = default)
        => GetRawAsync(ApiRoutes.Users.ProfileImage(id), ct);

    public Task<BaseResponse<bool>> SetProfileImageAsync(Guid id, SetProfileImageRequest request, CancellationToken ct = default)
        => PutAsync<SetProfileImageRequest, BaseResponse<bool>>(ApiRoutes.Users.ProfileImage(id), request, ct);

    public Task<BaseResponse<bool>> RemoveProfileImageAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>(ApiRoutes.Users.ProfileImage(id), ct);
}
