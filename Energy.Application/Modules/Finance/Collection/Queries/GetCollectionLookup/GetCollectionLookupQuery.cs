using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Collection.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Collection.Queries.GetCollectionLookup;

/// <summary>Collection lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetCollectionLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<CollectionLookupResponse>>>;
