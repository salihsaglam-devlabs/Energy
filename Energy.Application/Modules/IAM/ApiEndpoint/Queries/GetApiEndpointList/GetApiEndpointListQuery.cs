using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.System.Requests;
using Energy.Shared.Models.V1.System.Responses;
using MediatR;

namespace Energy.Application.Modules.IAM.ApiEndpoint.Queries.GetApiEndpointList;

/// <summary>GetApiEndpointList</summary>
public sealed record GetApiEndpointListQuery(PaginatedRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ApiEndpointResponse>>>;
