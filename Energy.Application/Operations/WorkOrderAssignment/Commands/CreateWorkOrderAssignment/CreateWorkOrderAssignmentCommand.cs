using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Requests;
using MediatR;

namespace Energy.Application.Operations.WorkOrderAssignment.Commands.CreateWorkOrderAssignment;

/// <summary>Yeni WorkOrderAssignment oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateWorkOrderAssignmentCommand(CreateWorkOrderAssignmentRequest Request)
    : IRequest<BaseResponse<Guid>>;
