using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.IAM.ApiEndpoint.Commands.CreateApiEndpoint;

/// <summary>CreateApiEndpoint</summary>
public sealed record CreateApiEndpointCommand(CreateApiEndpointRequest Request)
    : IRequest<BaseResponse<ApiEndpointResponse>>;
