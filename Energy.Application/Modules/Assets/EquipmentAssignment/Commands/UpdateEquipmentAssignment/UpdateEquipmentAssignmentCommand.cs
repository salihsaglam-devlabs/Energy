using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Requests;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentAssignment.Commands.UpdateEquipmentAssignment;

/// <summary>Var olan EquipmentAssignment kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateEquipmentAssignmentCommand(Guid Id, UpdateEquipmentAssignmentRequest Request)
    : IRequest<BaseResponse<bool>>;
