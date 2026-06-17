using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Assets.EquipmentAssignment.Requests;
using MediatR;

namespace Energy.Application.Assets.EquipmentAssignment.Commands.CreateEquipmentAssignment;

/// <summary>Yeni EquipmentAssignment oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateEquipmentAssignmentCommand(CreateEquipmentAssignmentRequest Request)
    : IRequest<BaseResponse<Guid>>;
