using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Web.Clients.Identity;

public interface IRoleApiClient
{
    Task<BaseResponse<PaginatedResponse<RoleSummaryResponse>>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default);
    Task<BaseResponse<RoleDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<RoleDetailResponse>> CreateAsync(CreateRoleRequest request, CancellationToken ct = default);
    Task<BaseResponse<RoleDetailResponse>> UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<RoleDetailResponse>> SetPermissionsAsync(Guid id, SetRolePermissionsRequest request, CancellationToken ct = default);
}
