using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeValue.Queries.GetMaterialAttributeValueLookup;

/// <summary>MaterialAttributeValue lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetMaterialAttributeValueLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<MaterialAttributeValueLookupResponse>>>;
