using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.MeasurementSheet.Queries.GetMeasurementSheetById;

/// <summary>Kimliğe göre MeasurementSheet detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetMeasurementSheetByIdQuery(Guid Id)
    : IRequest<BaseResponse<MeasurementSheetDetailResponse>>;
