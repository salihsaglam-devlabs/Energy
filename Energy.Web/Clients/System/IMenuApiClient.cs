using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;

namespace Energy.Web.Clients.System;

public interface IMenuApiClient
{
    Task<BaseResponse<PaginatedResponse<MenuResponse>>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default);
    Task<BaseResponse<MenuResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<MenuResponse>> CreateAsync(CreateMenuRequest request, CancellationToken ct = default);
    Task<BaseResponse<MenuResponse>> UpdateAsync(Guid id, UpdateMenuRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<MenuTreeNodeResponse>>> GetMyTreeAsync(CancellationToken ct = default);
}
