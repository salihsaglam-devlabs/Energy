using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Queries.GetMeasurementSheetLineLookup;

/// <summary>MeasurementSheetLine lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetMeasurementSheetLineLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<MeasurementSheetLineLookupResponse>>>;
