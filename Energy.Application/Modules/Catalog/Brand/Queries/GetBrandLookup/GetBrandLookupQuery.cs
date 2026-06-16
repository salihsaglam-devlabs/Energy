using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Brand.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.Brand.Queries.GetBrandLookup;

/// <summary>Brand lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetBrandLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<BrandLookupResponse>>>;
