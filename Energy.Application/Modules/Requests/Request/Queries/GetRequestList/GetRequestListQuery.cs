using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.Request.Requests;
using Energy.Shared.Models.V1.Requests.Request.Responses;
using MediatR;

namespace Energy.Application.Modules.Requests.Request.Queries.GetRequestList;

/// <summary>Sayfalanmış Request listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetRequestListQuery(GetRequestListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<RequestListResponse>>>;
