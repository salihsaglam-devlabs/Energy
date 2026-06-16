using Energy.Application.Modules.FieldOperations.MeasurementSheet.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.MeasurementSheet.Commands.DeleteMeasurementSheet;

/// <summary>
/// <see cref="DeleteMeasurementSheetCommand"/> handler'ı. <see cref="IMeasurementSheetService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteMeasurementSheetCommandHandler
    : IRequestHandler<DeleteMeasurementSheetCommand, BaseResponse<bool>>
{
    private readonly IMeasurementSheetService _service;

    public DeleteMeasurementSheetCommandHandler(IMeasurementSheetService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteMeasurementSheetCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
