using Energy.Application.Organization.EmployeeSkillAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Responses;
using MediatR;

namespace Energy.Application.Organization.EmployeeSkillAssignment.Queries.GetEmployeeSkillAssignmentById;

/// <summary>
/// <see cref="GetEmployeeSkillAssignmentByIdQuery"/> handler'ı. <see cref="IEmployeeSkillAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class GetEmployeeSkillAssignmentByIdQueryHandler
    : IRequestHandler<GetEmployeeSkillAssignmentByIdQuery, BaseResponse<EmployeeSkillAssignmentDetailResponse>>
{
    private readonly IEmployeeSkillAssignmentService _service;

    public GetEmployeeSkillAssignmentByIdQueryHandler(IEmployeeSkillAssignmentService service)
        => _service = service;

    public Task<BaseResponse<EmployeeSkillAssignmentDetailResponse>> Handle(
        GetEmployeeSkillAssignmentByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
