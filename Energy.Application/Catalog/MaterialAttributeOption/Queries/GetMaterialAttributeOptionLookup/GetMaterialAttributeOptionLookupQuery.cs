using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Responses;
using MediatR;

namespace Energy.Application.Catalog.MaterialAttributeOption.Queries.GetMaterialAttributeOptionLookup;

/// <summary>MaterialAttributeOption lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetMaterialAttributeOptionLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<MaterialAttributeOptionLookupResponse>>>;
