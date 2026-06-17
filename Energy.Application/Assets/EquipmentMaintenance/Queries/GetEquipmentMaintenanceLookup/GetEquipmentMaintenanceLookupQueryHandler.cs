using Energy.Application.Assets.EquipmentMaintenance.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentMaintenance.Queries.GetEquipmentMaintenanceLookup;

/// <summary>
/// <see cref="GetEquipmentMaintenanceLookupQuery"/> handler'ı. <see cref="IEquipmentMaintenanceLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetEquipmentMaintenanceLookupQueryHandler
    : IRequestHandler<GetEquipmentMaintenanceLookupQuery, BaseResponse<IReadOnlyList<EquipmentMaintenanceLookupResponse>>>
{
    private readonly IEquipmentMaintenanceLookupService _lookup;

    public GetEquipmentMaintenanceLookupQueryHandler(IEquipmentMaintenanceLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<EquipmentMaintenanceLookupResponse>>> Handle(
        GetEquipmentMaintenanceLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
