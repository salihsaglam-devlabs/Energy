using Energy.Application.Modules.Assets.EquipmentMaintenance.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentMaintenance.Commands.CreateEquipmentMaintenance;

/// <summary>
/// <see cref="CreateEquipmentMaintenanceCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IEquipmentMaintenanceService"/>'i orkestre eder.
/// </summary>
public sealed class CreateEquipmentMaintenanceCommandHandler
    : IRequestHandler<CreateEquipmentMaintenanceCommand, BaseResponse<Guid>>
{
    private readonly IEquipmentMaintenanceService _service;

    public CreateEquipmentMaintenanceCommandHandler(IEquipmentMaintenanceService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateEquipmentMaintenanceCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
