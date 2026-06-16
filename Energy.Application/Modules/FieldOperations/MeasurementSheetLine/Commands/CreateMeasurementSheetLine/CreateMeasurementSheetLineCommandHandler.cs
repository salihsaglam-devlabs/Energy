using Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.MeasurementSheetLine.Commands.CreateMeasurementSheetLine;

/// <summary>
/// <see cref="CreateMeasurementSheetLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IMeasurementSheetLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreateMeasurementSheetLineCommandHandler
    : IRequestHandler<CreateMeasurementSheetLineCommand, BaseResponse<Guid>>
{
    private readonly IMeasurementSheetLineService _service;

    public CreateMeasurementSheetLineCommandHandler(IMeasurementSheetLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateMeasurementSheetLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
