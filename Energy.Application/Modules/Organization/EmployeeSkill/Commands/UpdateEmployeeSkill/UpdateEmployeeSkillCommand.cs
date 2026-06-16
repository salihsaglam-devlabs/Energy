using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Requests;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeeSkill.Commands.UpdateEmployeeSkill;

/// <summary>Var olan EmployeeSkill kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateEmployeeSkillCommand(Guid Id, UpdateEmployeeSkillRequest Request)
    : IRequest<BaseResponse<bool>>;
