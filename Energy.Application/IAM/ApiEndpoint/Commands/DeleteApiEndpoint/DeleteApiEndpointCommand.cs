using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.IAM.ApiEndpoint.Commands.DeleteApiEndpoint;

/// <summary>DeleteApiEndpoint</summary>
public sealed record DeleteApiEndpointCommand(Guid Id)
    : IRequest<BaseResponse<bool>>;
