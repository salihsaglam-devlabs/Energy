using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.System;

public sealed class MenuApiClient : ApiClientBase, IMenuApiClient
{
    public MenuApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<MenuResponse>>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<MenuResponse>>>(
            $"{ApiRoutes.Menus.Base}?pageNumber={request.PageNumber}&pageSize={request.PageSize}", ct);

    public Task<BaseResponse<MenuResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<MenuResponse>>(ApiRoutes.Menus.ById(id), ct);

    public Task<BaseResponse<MenuResponse>> CreateAsync(CreateMenuRequest request, CancellationToken ct = default)
        => PostAsync<CreateMenuRequest, BaseResponse<MenuResponse>>(ApiRoutes.Menus.Base, request, ct);

    public Task<BaseResponse<MenuResponse>> UpdateAsync(Guid id, UpdateMenuRequest request, CancellationToken ct = default)
        => PutAsync<UpdateMenuRequest, BaseResponse<MenuResponse>>(ApiRoutes.Menus.ById(id), request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>(ApiRoutes.Menus.ById(id), ct);

    public Task<BaseResponse<IReadOnlyList<MenuTreeNodeResponse>>> GetMyTreeAsync(CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<MenuTreeNodeResponse>>>(ApiRoutes.Menus.Me, ct);
}
