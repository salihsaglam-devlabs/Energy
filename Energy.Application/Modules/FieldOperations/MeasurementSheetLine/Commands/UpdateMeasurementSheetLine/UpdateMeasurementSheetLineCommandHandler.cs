using Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Commands.UpdateMeasurementSheetLine;

/// <summary>
/// <see cref="UpdateMeasurementSheetLineCommand"/> handler'ı. <see cref="IMeasurementSheetLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateMeasurementSheetLineCommandHandler
    : IRequestHandler<UpdateMeasurementSheetLineCommand, BaseResponse<bool>>
{
    private readonly IMeasurementSheetLineService _service;

    public UpdateMeasurementSheetLineCommandHandler(IMeasurementSheetLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateMeasurementSheetLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
