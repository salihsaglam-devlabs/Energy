using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Requests;
using MediatR;

namespace Energy.Application.FieldOperations.MeasurementSheetLine.Commands.UpdateMeasurementSheetLine;

/// <summary>Var olan MeasurementSheetLine kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateMeasurementSheetLineCommand(Guid Id, UpdateMeasurementSheetLineRequest Request)
    : IRequest<BaseResponse<bool>>;
