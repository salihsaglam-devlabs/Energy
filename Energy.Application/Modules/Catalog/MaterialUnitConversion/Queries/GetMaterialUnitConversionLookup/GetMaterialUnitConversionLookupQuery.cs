using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Responses;
using MediatR;

namespace Energy.Application.Modules.Catalog.MaterialUnitConversion.Queries.GetMaterialUnitConversionLookup;

/// <summary>MaterialUnitConversion lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetMaterialUnitConversionLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<MaterialUnitConversionLookupResponse>>>;
