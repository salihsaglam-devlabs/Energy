using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Requests;
using MediatR;

namespace Energy.Application.Assets.EquipmentMaintenance.Commands.UpdateEquipmentMaintenance;

/// <summary>Var olan EquipmentMaintenance kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateEquipmentMaintenanceCommand(Guid Id, UpdateEquipmentMaintenanceRequest Request)
    : IRequest<BaseResponse<bool>>;
