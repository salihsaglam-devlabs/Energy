using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Responses;
using MediatR;

namespace Energy.Application.Finance.CollectionAllocation.Queries.GetCollectionAllocationById;

/// <summary>Kimliğe göre CollectionAllocation detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetCollectionAllocationByIdQuery(Guid Id)
    : IRequest<BaseResponse<CollectionAllocationDetailResponse>>;
