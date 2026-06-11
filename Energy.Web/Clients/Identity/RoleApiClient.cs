using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Identity;

public sealed class RoleApiClient : ApiClientBase, IRoleApiClient
{
    public RoleApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<RoleSummaryResponse>>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<RoleSummaryResponse>>>(
            $"{ApiRoutes.Roles.Base}?pageNumber={request.PageNumber}&pageSize={request.PageSize}&search={Uri.EscapeDataString(request.Search ?? string.Empty)}", ct);

    public Task<BaseResponse<RoleDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<RoleDetailResponse>>(ApiRoutes.Roles.ById(id), ct);

    public Task<BaseResponse<RoleDetailResponse>> CreateAsync(CreateRoleRequest request, CancellationToken ct = default)
        => PostAsync<CreateRoleRequest, BaseResponse<RoleDetailResponse>>(ApiRoutes.Roles.Base, request, ct);

    public Task<BaseResponse<RoleDetailResponse>> UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken ct = default)
        => PutAsync<UpdateRoleRequest, BaseResponse<RoleDetailResponse>>(ApiRoutes.Roles.ById(id), request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>(ApiRoutes.Roles.ById(id), ct);

    public Task<BaseResponse<RoleDetailResponse>> SetPermissionsAsync(Guid id, SetRolePermissionsRequest request, CancellationToken ct = default)
        => PutAsync<SetRolePermissionsRequest, BaseResponse<RoleDetailResponse>>(ApiRoutes.Roles.Permissions(id), request, ct);
}
