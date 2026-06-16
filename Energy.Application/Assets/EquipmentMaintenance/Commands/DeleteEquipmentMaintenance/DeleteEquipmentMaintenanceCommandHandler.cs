using Energy.Application.Assets.EquipmentMaintenance.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentMaintenance.Commands.DeleteEquipmentMaintenance;

/// <summary>
/// <see cref="DeleteEquipmentMaintenanceCommand"/> handler'ı. <see cref="IEquipmentMaintenanceService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteEquipmentMaintenanceCommandHandler
    : IRequestHandler<DeleteEquipmentMaintenanceCommand, BaseResponse<bool>>
{
    private readonly IEquipmentMaintenanceService _service;

    public DeleteEquipmentMaintenanceCommandHandler(IEquipmentMaintenanceService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteEquipmentMaintenanceCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
