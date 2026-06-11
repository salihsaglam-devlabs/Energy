using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.System;

public sealed class MenuApiClient : ApiClientBase, IMenuApiClient
{
    public MenuApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public Task<BaseResponse<PaginatedResponse<MenuResponse>>> GetMenusAsync(PaginatedRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<PaginatedResponse<MenuResponse>>>(ApiQueryString.Append(ApiRoutes.Menus.Base, request), cancellationToken);

    public Task<BaseResponse<IReadOnlyList<MenuResponse>>> GetMenuTreeAsync(CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<IReadOnlyList<MenuResponse>>>(ApiRoutes.Menus.Tree, cancellationToken);

    public Task<BaseResponse<MenuResponse>> GetMenuAsync(Guid id, CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<MenuResponse>>(ApiRoutes.Menus.ById(id), cancellationToken);

    public Task<BaseResponse<MenuResponse>> CreateMenuAsync(CreateMenuRequest request, CancellationToken cancellationToken = default)
        => PostAsync<CreateMenuRequest, BaseResponse<MenuResponse>>(ApiRoutes.Menus.Base, request, cancellationToken);

    public Task<BaseResponse<MenuResponse>> UpdateMenuAsync(Guid id, UpdateMenuRequest request, CancellationToken cancellationToken = default)
        => PutAsync<UpdateMenuRequest, BaseResponse<MenuResponse>>(ApiRoutes.Menus.ById(id), request, cancellationToken);

    public Task<BaseResponse<PaginatedResponse<PermissionResponse>>> GetMenuPermissionsAsync(
        Guid id,
        PaginatedRequest? request = null,
        CancellationToken cancellationToken = default)
        => GetAsync<BaseResponse<PaginatedResponse<PermissionResponse>>>(ApiQueryString.Append(ApiRoutes.Menus.Permissions(id), request), cancellationToken);

    public Task<BaseResponse<IReadOnlyList<PermissionResponse>>> SetMenuPermissionsAsync(
        Guid id,
        SetMenuPermissionsRequest request,
        CancellationToken cancellationToken = default)
        => PutAsync<SetMenuPermissionsRequest, BaseResponse<IReadOnlyList<PermissionResponse>>>(ApiRoutes.Menus.Permissions(id), request, cancellationToken);

    public Task<BaseResponse<Guid>> DeleteMenuAsync(Guid id, CancellationToken cancellationToken = default)
        => DeleteAsync<BaseResponse<Guid>>(ApiRoutes.Menus.ById(id), cancellationToken);

    public Task<BaseResponse<SeedResultResponse>> SeedDefaultsAsync(CancellationToken cancellationToken = default)
        => PostAsync<BaseResponse<SeedResultResponse>>(ApiRoutes.Menus.SeedDefaults, cancellationToken);
}
