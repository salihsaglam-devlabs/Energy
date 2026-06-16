using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Application.System.Services;
using MediatR;

namespace Energy.Application.IAM.ApiEndpoint.Commands.UpdateApiEndpoint;

/// <summary><see cref="UpdateApiEndpointCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class UpdateApiEndpointCommandHandler
    : IRequestHandler<UpdateApiEndpointCommand, BaseResponse<ApiEndpointResponse>>
{
    private readonly IApiEndpointService _endpoints;

    public UpdateApiEndpointCommandHandler(IApiEndpointService endpoints)
    {
        _endpoints = endpoints;
    }

    public async Task<BaseResponse<ApiEndpointResponse>> Handle(UpdateApiEndpointCommand request, CancellationToken ct)
    {
        var result = await _endpoints.UpdateAsync(request.Id, request.Request, ct);
        return BaseResponse<ApiEndpointResponse>.Success(result);
    }
}
