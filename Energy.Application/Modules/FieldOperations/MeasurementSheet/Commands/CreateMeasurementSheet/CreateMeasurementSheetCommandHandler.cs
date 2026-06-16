using Energy.Application.Modules.FieldOperations.MeasurementSheet.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.MeasurementSheet.Commands.CreateMeasurementSheet;

/// <summary>
/// <see cref="CreateMeasurementSheetCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IMeasurementSheetService"/>'i orkestre eder.
/// </summary>
public sealed class CreateMeasurementSheetCommandHandler
    : IRequestHandler<CreateMeasurementSheetCommand, BaseResponse<Guid>>
{
    private readonly IMeasurementSheetService _service;

    public CreateMeasurementSheetCommandHandler(IMeasurementSheetService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateMeasurementSheetCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
