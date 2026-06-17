using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentMaintenance.Queries.GetEquipmentMaintenanceById;

/// <summary>Kimliğe göre EquipmentMaintenance detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetEquipmentMaintenanceByIdQuery(Guid Id)
    : IRequest<BaseResponse<EquipmentMaintenanceDetailResponse>>;
