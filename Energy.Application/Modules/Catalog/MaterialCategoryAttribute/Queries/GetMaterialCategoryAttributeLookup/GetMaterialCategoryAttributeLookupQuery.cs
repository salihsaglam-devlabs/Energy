using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategoryAttribute.Queries.GetMaterialCategoryAttributeLookup;

/// <summary>MaterialCategoryAttribute lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetMaterialCategoryAttributeLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<MaterialCategoryAttributeLookupResponse>>>;
