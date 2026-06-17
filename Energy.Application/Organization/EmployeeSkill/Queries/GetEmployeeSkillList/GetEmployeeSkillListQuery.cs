using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Requests;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Responses;
using MediatR;

namespace Energy.Application.Organization.EmployeeSkill.Queries.GetEmployeeSkillList;

/// <summary>Sayfalanmış EmployeeSkill listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetEmployeeSkillListQuery(GetEmployeeSkillListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<EmployeeSkillListResponse>>>;
