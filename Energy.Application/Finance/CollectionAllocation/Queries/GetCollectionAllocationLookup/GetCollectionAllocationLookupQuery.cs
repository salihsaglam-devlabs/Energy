using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Responses;
using MediatR;

namespace Energy.Application.Finance.CollectionAllocation.Queries.GetCollectionAllocationLookup;

/// <summary>CollectionAllocation lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetCollectionAllocationLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<CollectionAllocationLookupResponse>>>;
