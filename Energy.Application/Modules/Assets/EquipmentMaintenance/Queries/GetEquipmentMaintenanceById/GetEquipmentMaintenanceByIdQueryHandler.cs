using Energy.Application.Modules.Assets.EquipmentMaintenance.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Responses;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentMaintenance.Queries.GetEquipmentMaintenanceById;

/// <summary>
/// <see cref="GetEquipmentMaintenanceByIdQuery"/> handler'ı. <see cref="IEquipmentMaintenanceService"/>'i orkestre eder.
/// </summary>
public sealed class GetEquipmentMaintenanceByIdQueryHandler
    : IRequestHandler<GetEquipmentMaintenanceByIdQuery, BaseResponse<EquipmentMaintenanceDetailResponse>>
{
    private readonly IEquipmentMaintenanceService _service;

    public GetEquipmentMaintenanceByIdQueryHandler(IEquipmentMaintenanceService service)
        => _service = service;

    public Task<BaseResponse<EquipmentMaintenanceDetailResponse>> Handle(
        GetEquipmentMaintenanceByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
