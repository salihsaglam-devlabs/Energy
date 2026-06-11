using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;

namespace Energy.Web.Clients.Identity;

public interface IRoleApiClient
{
    Task<BaseResponse<PaginatedResponse<RoleSummaryResponse>>> GetRolesAsync(PaginatedRequest? request = null, CancellationToken cancellationToken = default);

    Task<BaseResponse<RoleDetailResponse>> GetRoleAsync(Guid id, CancellationToken cancellationToken = default);

    Task<BaseResponse<RoleDetailResponse>> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);

    Task<BaseResponse<RoleDetailResponse>> UpdateRoleAsync(Guid id, UpdateRoleRequest request, CancellationToken cancellationToken = default);

    Task<BaseResponse<Guid>> DeleteRoleAsync(Guid id, CancellationToken cancellationToken = default);

    Task<BaseResponse<PaginatedResponse<PermissionResponse>>> GetRolePermissionsAsync(Guid id, PaginatedRequest? request = null, CancellationToken cancellationToken = default);

    Task<BaseResponse<IReadOnlyList<PermissionResponse>>> SetRolePermissionsAsync(Guid id, SetRolePermissionsRequest request, CancellationToken cancellationToken = default);

    Task<BaseResponse<PaginatedResponse<MenuResponse>>> GetRoleMenusAsync(Guid id, PaginatedRequest? request = null, CancellationToken cancellationToken = default);

    Task<BaseResponse<IReadOnlyList<MenuResponse>>> SetRoleMenusAsync(Guid id, SetRoleMenusRequest request, CancellationToken cancellationToken = default);
}
