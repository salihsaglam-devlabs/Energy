using Energy.Application.FieldOperations.MeasurementSheet.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.MeasurementSheet.Commands.UpdateMeasurementSheet;

/// <summary>
/// <see cref="UpdateMeasurementSheetCommand"/> handler'ı. <see cref="IMeasurementSheetService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateMeasurementSheetCommandHandler
    : IRequestHandler<UpdateMeasurementSheetCommand, BaseResponse<bool>>
{
    private readonly IMeasurementSheetService _service;

    public UpdateMeasurementSheetCommandHandler(IMeasurementSheetService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateMeasurementSheetCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
