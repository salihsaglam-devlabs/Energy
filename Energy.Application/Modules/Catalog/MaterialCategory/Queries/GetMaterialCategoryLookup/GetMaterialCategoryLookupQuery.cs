using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialCategory.Queries.GetMaterialCategoryLookup;

/// <summary>MaterialCategory lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetMaterialCategoryLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<MaterialCategoryLookupResponse>>>;
