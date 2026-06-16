using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Requests;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeeSkillAssignment.Commands.CreateEmployeeSkillAssignment;

/// <summary>Yeni EmployeeSkillAssignment oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateEmployeeSkillAssignmentCommand(CreateEmployeeSkillAssignmentRequest Request)
    : IRequest<BaseResponse<Guid>>;
