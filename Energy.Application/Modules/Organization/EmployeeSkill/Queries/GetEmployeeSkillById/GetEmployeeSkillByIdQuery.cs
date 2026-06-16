using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeeSkill.Queries.GetEmployeeSkillById;

/// <summary>Kimliğe göre EmployeeSkill detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetEmployeeSkillByIdQuery(Guid Id)
    : IRequest<BaseResponse<EmployeeSkillDetailResponse>>;
