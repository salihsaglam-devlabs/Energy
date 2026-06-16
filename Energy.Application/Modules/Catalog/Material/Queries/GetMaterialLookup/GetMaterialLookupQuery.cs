using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.Material.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.Material.Queries.GetMaterialLookup;

/// <summary>Material lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetMaterialLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<MaterialLookupResponse>>>;
