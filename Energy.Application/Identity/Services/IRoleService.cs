using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Shared.Models.V1.System.Responses;

namespace Energy.Application.Identity.Services;

public interface IRoleService
{
    Task<IReadOnlyList<RoleSummaryResponse>> GetRolesAsync(CancellationToken cancellationToken = default);

    Task<RoleDetailResponse> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<RoleDetailResponse> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);

    Task<RoleDetailResponse> UpdateRoleAsync(Guid id, UpdateRoleRequest request, CancellationToken cancellationToken = default);

    Task DeleteRoleAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PermissionResponse>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PermissionResponse>> SetRolePermissionsAsync(Guid roleId, IReadOnlyCollection<Guid> permissionIds, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MenuResponse>> GetRoleMenusAsync(Guid roleId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MenuResponse>> SetRoleMenusAsync(Guid roleId, IReadOnlyCollection<Guid> menuIds, CancellationToken cancellationToken = default);
}
