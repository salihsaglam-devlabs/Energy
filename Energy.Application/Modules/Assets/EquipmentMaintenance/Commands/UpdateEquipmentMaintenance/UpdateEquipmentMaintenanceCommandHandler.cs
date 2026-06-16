using Energy.Application.Modules.Assets.EquipmentMaintenance.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentMaintenance.Commands.UpdateEquipmentMaintenance;

/// <summary>
/// <see cref="UpdateEquipmentMaintenanceCommand"/> handler'ı. <see cref="IEquipmentMaintenanceService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateEquipmentMaintenanceCommandHandler
    : IRequestHandler<UpdateEquipmentMaintenanceCommand, BaseResponse<bool>>
{
    private readonly IEquipmentMaintenanceService _service;

    public UpdateEquipmentMaintenanceCommandHandler(IEquipmentMaintenanceService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateEquipmentMaintenanceCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
