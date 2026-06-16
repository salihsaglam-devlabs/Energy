using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Requests;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.MeasurementSheetLine.Queries.GetMeasurementSheetLineList;

/// <summary>Sayfalanmış MeasurementSheetLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetMeasurementSheetLineListQuery(GetMeasurementSheetLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<MeasurementSheetLineListResponse>>>;
