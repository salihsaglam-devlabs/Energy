using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Assets.EquipmentMaintenance.Commands.DeleteEquipmentMaintenance;

/// <summary>EquipmentMaintenance kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteEquipmentMaintenanceCommand(Guid Id) : IRequest<BaseResponse<bool>>;
