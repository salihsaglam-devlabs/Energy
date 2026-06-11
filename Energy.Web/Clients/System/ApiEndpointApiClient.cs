using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.System;

public sealed class ApiEndpointApiClient : ApiClientBase, IApiEndpointApiClient
{
    public ApiEndpointApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ApiEndpointResponse>>> GetAllAsync(PaginatedRequest request, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ApiEndpointResponse>>>(
            $"{ApiRoutes.ApiEndpoints.Base}?pageNumber={request.PageNumber}&pageSize={request.PageSize}", ct);

    public Task<BaseResponse<ApiEndpointResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<ApiEndpointResponse>>(ApiRoutes.ApiEndpoints.ById(id), ct);

    public Task<BaseResponse<ApiEndpointResponse>> CreateAsync(CreateApiEndpointRequest request, CancellationToken ct = default)
        => PostAsync<CreateApiEndpointRequest, BaseResponse<ApiEndpointResponse>>(ApiRoutes.ApiEndpoints.Base, request, ct);

    public Task<BaseResponse<ApiEndpointResponse>> UpdateAsync(Guid id, UpdateApiEndpointRequest request, CancellationToken ct = default)
        => PutAsync<UpdateApiEndpointRequest, BaseResponse<ApiEndpointResponse>>(ApiRoutes.ApiEndpoints.ById(id), request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>(ApiRoutes.ApiEndpoints.ById(id), ct);
}
