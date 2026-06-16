using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Requests.RequestType.Requests;
using Energy.Shared.Models.V1.Requests.RequestType.Responses;
using MediatR;

namespace Energy.Application.Modules.Requests.RequestType.Queries.GetRequestTypeList;

/// <summary>Sayfalanmış RequestType listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetRequestTypeListQuery(GetRequestTypeListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<RequestTypeListResponse>>>;
