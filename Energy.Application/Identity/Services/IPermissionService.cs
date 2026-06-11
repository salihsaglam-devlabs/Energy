using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Application.Identity.Services;

public interface IPermissionService
{
    Task<IReadOnlyList<PermissionResponse>> GetPermissionsAsync(CancellationToken cancellationToken = default);

    Task<PermissionResponse> GetPermissionByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PermissionResponse> CreatePermissionAsync(CreatePermissionRequest request, CancellationToken cancellationToken = default);

    Task<PermissionResponse> UpdatePermissionAsync(Guid id, UpdatePermissionRequest request, CancellationToken cancellationToken = default);

    Task DeletePermissionAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SeedResultResponse> SeedDefaultPermissionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Ensures every supplied permission code exists in the database. Codes that
    /// already exist are left untouched; missing ones are inserted with a name
    /// derived from the localization catalog. Used so any
    /// <c>[Authorize(Policy = "...")]</c> attribute discovered in controllers
    /// gets auto-registered on startup.
    /// </summary>
    Task<SeedResultResponse> SeedPermissionCodesAsync(IEnumerable<string> codes, CancellationToken cancellationToken = default);
}

