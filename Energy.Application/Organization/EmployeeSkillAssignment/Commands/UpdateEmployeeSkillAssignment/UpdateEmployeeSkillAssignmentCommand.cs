using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Requests;
using MediatR;

namespace Energy.Application.Organization.EmployeeSkillAssignment.Commands.UpdateEmployeeSkillAssignment;

/// <summary>Var olan EmployeeSkillAssignment kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateEmployeeSkillAssignmentCommand(Guid Id, UpdateEmployeeSkillAssignmentRequest Request)
    : IRequest<BaseResponse<bool>>;
