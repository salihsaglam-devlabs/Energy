using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Requests;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeeSkill.Commands.CreateEmployeeSkill;

/// <summary>Yeni EmployeeSkill oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateEmployeeSkillCommand(CreateEmployeeSkillRequest Request)
    : IRequest<BaseResponse<Guid>>;
