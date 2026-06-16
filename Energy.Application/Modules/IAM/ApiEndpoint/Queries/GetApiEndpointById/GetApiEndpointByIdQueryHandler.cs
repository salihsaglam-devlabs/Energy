using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Application.System.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.ApiEndpoint.Queries.GetApiEndpointById;

/// <summary><see cref="GetApiEndpointByIdQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetApiEndpointByIdQueryHandler
    : IRequestHandler<GetApiEndpointByIdQuery, BaseResponse<ApiEndpointResponse>>
{
    private readonly IApiEndpointService _endpoints;

    public GetApiEndpointByIdQueryHandler(IApiEndpointService endpoints)
    {
        _endpoints = endpoints;
    }

    public async Task<BaseResponse<ApiEndpointResponse>> Handle(GetApiEndpointByIdQuery request, CancellationToken ct)
    {
        var result = await _endpoints.GetByIdAsync(request.Id, ct);
        return result is null
            ? BaseResponse<ApiEndpointResponse>.Failure("Endpoint not found.")
            : BaseResponse<ApiEndpointResponse>.Success(result);
    }
}
