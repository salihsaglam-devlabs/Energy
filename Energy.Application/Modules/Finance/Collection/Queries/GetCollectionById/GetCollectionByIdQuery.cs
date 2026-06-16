using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Collection.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Collection.Queries.GetCollectionById;

/// <summary>Kimliğe göre Collection detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetCollectionByIdQuery(Guid Id)
    : IRequest<BaseResponse<CollectionDetailResponse>>;
