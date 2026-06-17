using Energy.Application.FieldOperations.MeasurementSheetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.MeasurementSheetLine.Commands.DeleteMeasurementSheetLine;

/// <summary>
/// <see cref="DeleteMeasurementSheetLineCommand"/> handler'ı. <see cref="IMeasurementSheetLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteMeasurementSheetLineCommandHandler
    : IRequestHandler<DeleteMeasurementSheetLineCommand, BaseResponse<bool>>
{
    private readonly IMeasurementSheetLineService _service;

    public DeleteMeasurementSheetLineCommandHandler(IMeasurementSheetLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteMeasurementSheetLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
