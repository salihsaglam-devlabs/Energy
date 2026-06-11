using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Web.Clients.Identity;

public interface IUserApiClient
{
    Task<BaseResponse<PaginatedResponse<UserSummaryResponse>>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default);
    Task<BaseResponse<UserDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<UserDetailResponse>> CreateAsync(CreateUserRequest request, CancellationToken ct = default);
    Task<BaseResponse<UserDetailResponse>> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<bool>> ChangePasswordAsync(Guid id, ChangePasswordRequest request, CancellationToken ct = default);
    Task<BaseResponse<UserAccessResponse>> GetAccessAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<UserAccessResponse>> SetAccessAsync(Guid id, SetUserAccessRequest request, CancellationToken ct = default);

    Task<(byte[] Content, string ContentType, int StatusCode)> GetProfileImageAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<bool>> SetProfileImageAsync(Guid id, SetProfileImageRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> RemoveProfileImageAsync(Guid id, CancellationToken ct = default);
}
