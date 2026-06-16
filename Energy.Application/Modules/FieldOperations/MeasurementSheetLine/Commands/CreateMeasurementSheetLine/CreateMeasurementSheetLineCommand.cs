using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheetLine.Requests;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Commands.CreateMeasurementSheetLine;

/// <summary>Yeni MeasurementSheetLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateMeasurementSheetLineCommand(CreateMeasurementSheetLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
