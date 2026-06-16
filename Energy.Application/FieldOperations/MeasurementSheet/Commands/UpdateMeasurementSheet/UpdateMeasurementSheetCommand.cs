using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Requests;
using MediatR;

namespace Energy.Application.FieldOperations.MeasurementSheet.Commands.UpdateMeasurementSheet;

/// <summary>Var olan MeasurementSheet kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateMeasurementSheetCommand(Guid Id, UpdateMeasurementSheetRequest Request)
    : IRequest<BaseResponse<bool>>;
