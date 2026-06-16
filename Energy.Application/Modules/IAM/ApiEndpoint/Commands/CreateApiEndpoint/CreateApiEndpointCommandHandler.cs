using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Application.System.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.ApiEndpoint.Commands.CreateApiEndpoint;

/// <summary><see cref="CreateApiEndpointCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class CreateApiEndpointCommandHandler
    : IRequestHandler<CreateApiEndpointCommand, BaseResponse<ApiEndpointResponse>>
{
    private readonly IApiEndpointService _endpoints;

    public CreateApiEndpointCommandHandler(IApiEndpointService endpoints)
    {
        _endpoints = endpoints;
    }

    public async Task<BaseResponse<ApiEndpointResponse>> Handle(CreateApiEndpointCommand request, CancellationToken ct)
    {
        var result = await _endpoints.CreateAsync(request.Request, ct);
        return BaseResponse<ApiEndpointResponse>.Success(result);
    }
}
