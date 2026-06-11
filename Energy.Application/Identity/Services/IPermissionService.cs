using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Application.Identity.Services;

public interface IPermissionService
{
    Task<IReadOnlyList<PermissionResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PermissionResponse?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>Sync the catalog from <c>Energy.Shared.Identity.Permissions.PermissionCatalog</c>.</summary>
    Task<int> SyncCatalogAsync(CancellationToken cancellationToken = default);
}
