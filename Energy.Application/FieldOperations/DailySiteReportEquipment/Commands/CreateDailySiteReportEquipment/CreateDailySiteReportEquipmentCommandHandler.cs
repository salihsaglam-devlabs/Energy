using Energy.Application.FieldOperations.DailySiteReportEquipment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportEquipment.Commands.CreateDailySiteReportEquipment;

/// <summary>
/// <see cref="CreateDailySiteReportEquipmentCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IDailySiteReportEquipmentService"/>'i orkestre eder.
/// </summary>
public sealed class CreateDailySiteReportEquipmentCommandHandler
    : IRequestHandler<CreateDailySiteReportEquipmentCommand, BaseResponse<Guid>>
{
    private readonly IDailySiteReportEquipmentService _service;

    public CreateDailySiteReportEquipmentCommandHandler(IDailySiteReportEquipmentService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateDailySiteReportEquipmentCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
