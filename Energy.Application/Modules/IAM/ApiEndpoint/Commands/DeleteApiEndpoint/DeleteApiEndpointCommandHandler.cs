using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using Energy.Application.System.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.ApiEndpoint.Commands.DeleteApiEndpoint;

/// <summary><see cref="DeleteApiEndpointCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class DeleteApiEndpointCommandHandler
    : IRequestHandler<DeleteApiEndpointCommand, BaseResponse<bool>>
{
    private readonly IApiEndpointService _endpoints;

    public DeleteApiEndpointCommandHandler(IApiEndpointService endpoints)
    {
        _endpoints = endpoints;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteApiEndpointCommand request, CancellationToken ct)
    {
        var result = await _endpoints.DeleteAsync(request.Id, ct);
        return BaseResponse<bool>.Success(result);
    }
}
