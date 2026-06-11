using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Identity;

public sealed class RoleApiClient : ApiClientBase, IRoleApiClient
{
    public RoleApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<BaseResponse<PaginatedResponse<RoleSummaryResponse>>> GetRolesAsync(PaginatedRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<PaginatedResponse<RoleSummaryResponse>>>(ApiQueryString.Append(ApiRoutes.Roles.Base, request), cancellationToken);

    public Task<BaseResponse<RoleDetailResponse>> GetRoleAsync(Guid id, CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<RoleDetailResponse>>(ApiRoutes.Roles.ById(id), cancellationToken);

    public Task<BaseResponse<RoleDetailResponse>> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default)
        => PostAsync<CreateRoleRequest, BaseResponse<RoleDetailResponse>>(ApiRoutes.Roles.Base, request, cancellationToken);

    public Task<BaseResponse<RoleDetailResponse>> UpdateRoleAsync(Guid id, UpdateRoleRequest request, CancellationToken cancellationToken = default)
        => PutAsync<UpdateRoleRequest, BaseResponse<RoleDetailResponse>>(ApiRoutes.Roles.ById(id), request, cancellationToken);

    public Task<BaseResponse<Guid>> DeleteRoleAsync(Guid id, CancellationToken cancellationToken = default)
        => DeleteAsync<BaseResponse<Guid>>(ApiRoutes.Roles.ById(id), cancellationToken);

    public Task<BaseResponse<PaginatedResponse<PermissionResponse>>> GetRolePermissionsAsync(Guid id, PaginatedRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<PaginatedResponse<PermissionResponse>>>(ApiQueryString.Append(ApiRoutes.Roles.Permissions(id), request), cancellationToken);

    public Task<BaseResponse<IReadOnlyList<PermissionResponse>>> SetRolePermissionsAsync(Guid id, SetRolePermissionsRequest request, CancellationToken cancellationToken = default)
        => PutAsync<SetRolePermissionsRequest, BaseResponse<IReadOnlyList<PermissionResponse>>>(ApiRoutes.Roles.Permissions(id), request, cancellationToken);

    public Task<BaseResponse<PaginatedResponse<MenuResponse>>> GetRoleMenusAsync(Guid id, PaginatedRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<PaginatedResponse<MenuResponse>>>(ApiQueryString.Append(ApiRoutes.Roles.Menus(id), request), cancellationToken);

    public Task<BaseResponse<IReadOnlyList<MenuResponse>>> SetRoleMenusAsync(Guid id, SetRoleMenusRequest request, CancellationToken cancellationToken = default)
        => PutAsync<SetRoleMenusRequest, BaseResponse<IReadOnlyList<MenuResponse>>>(ApiRoutes.Roles.Menus(id), request, cancellationToken);
}
