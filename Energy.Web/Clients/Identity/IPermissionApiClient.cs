using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Web.Clients.Identity;

public interface IPermissionApiClient
{
    Task<BaseResponse<IReadOnlyList<PermissionResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<BaseResponse<PermissionResponse>> GetByCodeAsync(string code, CancellationToken ct = default);
}
