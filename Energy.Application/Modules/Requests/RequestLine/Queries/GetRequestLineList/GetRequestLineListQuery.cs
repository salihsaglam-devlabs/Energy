using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestLine.Requests;
using Energy.Shared.Models.V1.Requests.RequestLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Requests.RequestLine.Queries.GetRequestLineList;

/// <summary>Sayfalanmış RequestLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetRequestLineListQuery(GetRequestLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<RequestLineListResponse>>>;
