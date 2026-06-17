using Energy.Application.Assets.EquipmentMaintenance.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentMaintenance.Queries.GetEquipmentMaintenanceList;

/// <summary>
/// <see cref="GetEquipmentMaintenanceListQuery"/> handler'ı. <see cref="IEquipmentMaintenanceService"/>'i orkestre eder.
/// </summary>
public sealed class GetEquipmentMaintenanceListQueryHandler
    : IRequestHandler<GetEquipmentMaintenanceListQuery, BaseResponse<PaginatedResponse<EquipmentMaintenanceListResponse>>>
{
    private readonly IEquipmentMaintenanceService _service;

    public GetEquipmentMaintenanceListQueryHandler(IEquipmentMaintenanceService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<EquipmentMaintenanceListResponse>>> Handle(
        GetEquipmentMaintenanceListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
