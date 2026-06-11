using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Identity;

public sealed class AuthApiClient : ApiClientBase, IAuthApiClient
{
    public AuthApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<AuthTokenResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
        => PostAsync<LoginRequest, BaseResponse<AuthTokenResponse>>(ApiRoutes.Auth.Login, request, ct);
}
