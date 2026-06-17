using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Requests;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;
using MediatR;

namespace Energy.Application.Organization.EmployeeSkillAssignment.Queries.GetEmployeeSkillAssignmentList;

/// <summary>Sayfalanmış EmployeeSkillAssignment listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetEmployeeSkillAssignmentListQuery(GetEmployeeSkillAssignmentListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<EmployeeSkillAssignmentListResponse>>>;
