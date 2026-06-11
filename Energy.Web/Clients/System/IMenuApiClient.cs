using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;

namespace Energy.Web.Clients.System;

public interface IMenuApiClient
{
    Task<BaseResponse<PaginatedResponse<MenuResponse>>> GetMenusAsync(PaginatedRequest? request = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the hierarchical menu tree (roots with nested children).
    /// Names are localized to the current request culture by the API.
    /// </summary>
    Task<BaseResponse<IReadOnlyList<MenuResponse>>> GetMenuTreeAsync(CancellationToken cancellationToken = default);

    Task<BaseResponse<MenuResponse>> GetMenuAsync(Guid id, CancellationToken cancellationToken = default);

    Task<BaseResponse<MenuResponse>> CreateMenuAsync(CreateMenuRequest request, CancellationToken cancellationToken = default);

    Task<BaseResponse<MenuResponse>> UpdateMenuAsync(Guid id, UpdateMenuRequest request, CancellationToken cancellationToken = default);

    Task<BaseResponse<PaginatedResponse<PermissionResponse>>> GetMenuPermissionsAsync(
        Guid id,
        PaginatedRequest? request = null,
        CancellationToken cancellationToken = default);

    Task<BaseResponse<IReadOnlyList<PermissionResponse>>> SetMenuPermissionsAsync(
        Guid id,
        SetMenuPermissionsRequest request,
        CancellationToken cancellationToken = default);

    Task<BaseResponse<Guid>> DeleteMenuAsync(Guid id, CancellationToken cancellationToken = default);

    Task<BaseResponse<SeedResultResponse>> SeedDefaultsAsync(CancellationToken cancellationToken = default);
}
