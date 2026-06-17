using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportMaterial.Queries.GetDailySiteReportMaterialLookup;

/// <summary>DailySiteReportMaterial lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetDailySiteReportMaterialLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<DailySiteReportMaterialLookupResponse>>>;
