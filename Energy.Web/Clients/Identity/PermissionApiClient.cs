using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Identity;

public sealed class PermissionApiClient : ApiClientBase, IPermissionApiClient
{
    public PermissionApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<BaseResponse<PaginatedResponse<PermissionResponse>>> GetPermissionsAsync(PaginatedRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<PaginatedResponse<PermissionResponse>>>(ApiQueryString.Append(ApiRoutes.Permissions.Base, request), cancellationToken);

    public Task<BaseResponse<PermissionResponse>> GetPermissionAsync(Guid id, CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<PermissionResponse>>(ApiRoutes.Permissions.ById(id), cancellationToken);

    public Task<BaseResponse<PermissionResponse>> CreatePermissionAsync(CreatePermissionRequest request, CancellationToken cancellationToken = default)
        => PostAsync<CreatePermissionRequest, BaseResponse<PermissionResponse>>(ApiRoutes.Permissions.Base, request, cancellationToken);

    public Task<BaseResponse<PermissionResponse>> UpdatePermissionAsync(Guid id, UpdatePermissionRequest request, CancellationToken cancellationToken = default)
        => PutAsync<UpdatePermissionRequest, BaseResponse<PermissionResponse>>(ApiRoutes.Permissions.ById(id), request, cancellationToken);

    public Task<BaseResponse<Guid>> DeletePermissionAsync(Guid id, CancellationToken cancellationToken = default)
        => DeleteAsync<BaseResponse<Guid>>(ApiRoutes.Permissions.ById(id), cancellationToken);

    public Task<BaseResponse<SeedResultResponse>> SeedDefaultsAsync(CancellationToken cancellationToken = default)
        => PostAsync<BaseResponse<SeedResultResponse>>(ApiRoutes.Permissions.SeedDefaults, cancellationToken);
}
