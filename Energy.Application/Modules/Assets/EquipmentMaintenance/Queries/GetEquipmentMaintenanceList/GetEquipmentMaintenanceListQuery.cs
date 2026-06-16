using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Requests;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Responses;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentMaintenance.Queries.GetEquipmentMaintenanceList;

/// <summary>Sayfalanmış EquipmentMaintenance listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetEquipmentMaintenanceListQuery(GetEquipmentMaintenanceListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<EquipmentMaintenanceListResponse>>>;
