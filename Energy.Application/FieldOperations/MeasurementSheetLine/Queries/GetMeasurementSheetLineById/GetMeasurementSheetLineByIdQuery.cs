using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.MeasurementSheetLine.Queries.GetMeasurementSheetLineById;

/// <summary>Kimliğe göre MeasurementSheetLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetMeasurementSheetLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<MeasurementSheetLineDetailResponse>>;
