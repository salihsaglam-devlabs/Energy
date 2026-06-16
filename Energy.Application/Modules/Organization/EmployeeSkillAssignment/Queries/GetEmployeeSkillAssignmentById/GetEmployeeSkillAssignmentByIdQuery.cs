using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeeSkillAssignment.Queries.GetEmployeeSkillAssignmentById;

/// <summary>Kimliğe göre EmployeeSkillAssignment detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetEmployeeSkillAssignmentByIdQuery(Guid Id)
    : IRequest<BaseResponse<EmployeeSkillAssignmentDetailResponse>>;
