using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeeSkill.Commands.DeleteEmployeeSkill;

/// <summary>EmployeeSkill kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteEmployeeSkillCommand(Guid Id) : IRequest<BaseResponse<bool>>;
