using Energy.Application.FieldOperations.DailySiteReportEquipment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportEquipment.Commands.UpdateDailySiteReportEquipment;

/// <summary>
/// <see cref="UpdateDailySiteReportEquipmentCommand"/> handler'ı. <see cref="IDailySiteReportEquipmentService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateDailySiteReportEquipmentCommandHandler
    : IRequestHandler<UpdateDailySiteReportEquipmentCommand, BaseResponse<bool>>
{
    private readonly IDailySiteReportEquipmentService _service;

    public UpdateDailySiteReportEquipmentCommandHandler(IDailySiteReportEquipmentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateDailySiteReportEquipmentCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
