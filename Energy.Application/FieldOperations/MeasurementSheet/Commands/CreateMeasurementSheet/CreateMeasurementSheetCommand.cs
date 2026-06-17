using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.MeasurementSheet.Requests;
using MediatR;

namespace Energy.Application.FieldOperations.MeasurementSheet.Commands.CreateMeasurementSheet;

/// <summary>Yeni MeasurementSheet oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateMeasurementSheetCommand(CreateMeasurementSheetRequest Request)
    : IRequest<BaseResponse<Guid>>;
