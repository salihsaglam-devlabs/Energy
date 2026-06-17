using Energy.Application.FieldOperations.DailySiteReportEquipment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.FieldOperations.DailySiteReportEquipment.Commands.DeleteDailySiteReportEquipment;

/// <summary>
/// <see cref="DeleteDailySiteReportEquipmentCommand"/> handler'ı. <see cref="IDailySiteReportEquipmentService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteDailySiteReportEquipmentCommandHandler
    : IRequestHandler<DeleteDailySiteReportEquipmentCommand, BaseResponse<bool>>
{
    private readonly IDailySiteReportEquipmentService _service;

    public DeleteDailySiteReportEquipmentCommandHandler(IDailySiteReportEquipmentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteDailySiteReportEquipmentCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
