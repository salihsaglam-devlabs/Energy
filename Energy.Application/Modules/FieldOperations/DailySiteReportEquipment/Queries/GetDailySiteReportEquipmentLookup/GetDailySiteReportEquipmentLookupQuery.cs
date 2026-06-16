using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportEquipment.Queries.GetDailySiteReportEquipmentLookup;

/// <summary>DailySiteReportEquipment lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetDailySiteReportEquipmentLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<DailySiteReportEquipmentLookupResponse>>>;
