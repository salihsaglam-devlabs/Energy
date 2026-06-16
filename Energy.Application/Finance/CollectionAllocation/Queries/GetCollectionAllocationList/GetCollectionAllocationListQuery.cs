using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Requests;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Responses;
using MediatR;

namespace Energy.Application.Finance.CollectionAllocation.Queries.GetCollectionAllocationList;

/// <summary>Sayfalanmış CollectionAllocation listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetCollectionAllocationListQuery(GetCollectionAllocationListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<CollectionAllocationListResponse>>>;
