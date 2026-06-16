using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.MeasurementSheetLine.Commands.DeleteMeasurementSheetLine;

/// <summary>MeasurementSheetLine kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteMeasurementSheetLineCommand(Guid Id) : IRequest<BaseResponse<bool>>;
