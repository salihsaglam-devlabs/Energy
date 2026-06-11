using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;

namespace Energy.Application.Identity.Services;

public interface IRoleService
{
    Task<PaginatedResponse<RoleSummaryResponse>> GetAllAsync(PaginatedRequest request, CancellationToken cancellationToken = default);
    Task<RoleDetailResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RoleDetailResponse> CreateAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);
    Task<RoleDetailResponse> UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RoleDetailResponse> SetPermissionsAsync(Guid id, SetRolePermissionsRequest request, CancellationToken cancellationToken = default);
}
