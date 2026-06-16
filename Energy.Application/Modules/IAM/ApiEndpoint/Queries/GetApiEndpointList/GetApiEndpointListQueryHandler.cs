using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Application.System.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.ApiEndpoint.Queries.GetApiEndpointList;

/// <summary><see cref="GetApiEndpointListQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetApiEndpointListQueryHandler
    : IRequestHandler<GetApiEndpointListQuery, BaseResponse<PaginatedResponse<ApiEndpointResponse>>>
{
    private readonly IApiEndpointService _endpoints;

    public GetApiEndpointListQueryHandler(IApiEndpointService endpoints)
    {
        _endpoints = endpoints;
    }

    public async Task<BaseResponse<PaginatedResponse<ApiEndpointResponse>>> Handle(GetApiEndpointListQuery request, CancellationToken ct)
    {
        var result = await _endpoints.GetAllAsync(request.Request, ct);
        return BaseResponse<PaginatedResponse<ApiEndpointResponse>>.Success(result);
    }
}
