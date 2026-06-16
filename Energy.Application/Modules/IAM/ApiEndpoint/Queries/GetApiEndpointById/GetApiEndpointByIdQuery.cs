using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.Modules.IAM.ApiEndpoint.Queries.GetApiEndpointById;

/// <summary>GetApiEndpointById</summary>
public sealed record GetApiEndpointByIdQuery(Guid Id)
    : IRequest<BaseResponse<ApiEndpointResponse>>;
