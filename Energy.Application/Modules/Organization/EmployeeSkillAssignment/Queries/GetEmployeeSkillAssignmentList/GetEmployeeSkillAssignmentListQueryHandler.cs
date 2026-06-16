using Energy.Application.Modules.Organization.EmployeeSkillAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeeSkillAssignment.Queries.GetEmployeeSkillAssignmentList;

/// <summary>
/// <see cref="GetEmployeeSkillAssignmentListQuery"/> handler'ı. <see cref="IEmployeeSkillAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class GetEmployeeSkillAssignmentListQueryHandler
    : IRequestHandler<GetEmployeeSkillAssignmentListQuery, BaseResponse<PaginatedResponse<EmployeeSkillAssignmentListResponse>>>
{
    private readonly IEmployeeSkillAssignmentService _service;

    public GetEmployeeSkillAssignmentListQueryHandler(IEmployeeSkillAssignmentService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<EmployeeSkillAssignmentListResponse>>> Handle(
        GetEmployeeSkillAssignmentListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
