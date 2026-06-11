using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Identity;

public sealed class PermissionApiClient : ApiClientBase, IPermissionApiClient
{
    public PermissionApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<IReadOnlyList<PermissionResponse>>> GetAllAsync(CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<PermissionResponse>>>(ApiRoutes.Permissions.Base, ct);

    public Task<BaseResponse<PermissionResponse>> GetByCodeAsync(string code, CancellationToken ct = default)
        => GetAsync<BaseResponse<PermissionResponse>>(ApiRoutes.Permissions.ByCode(code), ct);
}
