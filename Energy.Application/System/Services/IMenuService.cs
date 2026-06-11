using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;

namespace Energy.Application.System.Services;

public interface IMenuService
{
    Task<IReadOnlyList<MenuResponse>> GetMenusAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns root menus with their children populated recursively.
    /// Names are resolved to the current request culture.
    /// </summary>
    Task<IReadOnlyList<MenuResponse>> GetMenuTreeAsync(CancellationToken cancellationToken = default);

    Task<MenuResponse> GetMenuByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<MenuResponse> CreateMenuAsync(CreateMenuRequest request, CancellationToken cancellationToken = default);

    Task<MenuResponse> UpdateMenuAsync(Guid id, UpdateMenuRequest request, CancellationToken cancellationToken = default);

    Task DeleteMenuAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PermissionResponse>> GetMenuPermissionsAsync(Guid menuId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PermissionResponse>> SetMenuPermissionsAsync(
        Guid menuId,
        IReadOnlyCollection<Guid> permissionIds,
        CancellationToken cancellationToken = default);

    Task<SeedResultResponse> SeedDefaultMenusAsync(CancellationToken cancellationToken = default);
}
