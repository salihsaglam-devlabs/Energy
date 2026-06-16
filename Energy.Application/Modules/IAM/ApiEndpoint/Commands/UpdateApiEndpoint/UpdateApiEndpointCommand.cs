using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.Modules.IAM.ApiEndpoint.Commands.UpdateApiEndpoint;

/// <summary>UpdateApiEndpoint</summary>
public sealed record UpdateApiEndpointCommand(Guid Id, UpdateApiEndpointRequest Request)
    : IRequest<BaseResponse<ApiEndpointResponse>>;
