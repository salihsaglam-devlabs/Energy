using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Web.Clients.Identity;

public interface IAuthApiClient
{
    Task<BaseResponse<AuthTokenResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
}
