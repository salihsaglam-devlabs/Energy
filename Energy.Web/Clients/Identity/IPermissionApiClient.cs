using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Web.Clients.Identity;

public interface IPermissionApiClient
{
    Task<BaseResponse<PaginatedResponse<PermissionResponse>>> GetPermissionsAsync(PaginatedRequest? request = null, CancellationToken cancellationToken = default);

    Task<BaseResponse<PermissionResponse>> GetPermissionAsync(Guid id, CancellationToken cancellationToken = default);

    Task<BaseResponse<PermissionResponse>> CreatePermissionAsync(CreatePermissionRequest request, CancellationToken cancellationToken = default);

    Task<BaseResponse<PermissionResponse>> UpdatePermissionAsync(Guid id, UpdatePermissionRequest request, CancellationToken cancellationToken = default);

    Task<BaseResponse<Guid>> DeletePermissionAsync(Guid id, CancellationToken cancellationToken = default);

    Task<BaseResponse<SeedResultResponse>> SeedDefaultsAsync(CancellationToken cancellationToken = default);
}
