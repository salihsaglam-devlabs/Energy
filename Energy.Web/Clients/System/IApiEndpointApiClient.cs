using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;

namespace Energy.Web.Clients.System;

public interface IApiEndpointApiClient
{
    Task<BaseResponse<PaginatedResponse<ApiEndpointResponse>>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default);
    Task<BaseResponse<ApiEndpointResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<ApiEndpointResponse>> CreateAsync(CreateApiEndpointRequest request, CancellationToken ct = default);
    Task<BaseResponse<ApiEndpointResponse>> UpdateAsync(Guid id, UpdateApiEndpointRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}
