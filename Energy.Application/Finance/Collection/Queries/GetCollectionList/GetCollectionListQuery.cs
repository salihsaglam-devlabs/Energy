using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Collection.Requests;
using Energy.Shared.Models.V1.Finance.Collection.Responses;
using MediatR;

namespace Energy.Application.Finance.Collection.Queries.GetCollectionList;

/// <summary>Sayfalanmış Collection listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetCollectionListQuery(GetCollectionListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<CollectionListResponse>>>;
