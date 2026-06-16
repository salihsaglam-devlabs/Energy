using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Assets.EquipmentAssignment.Commands.DeleteEquipmentAssignment;

/// <summary>EquipmentAssignment kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteEquipmentAssignmentCommand(Guid Id) : IRequest<BaseResponse<bool>>;
