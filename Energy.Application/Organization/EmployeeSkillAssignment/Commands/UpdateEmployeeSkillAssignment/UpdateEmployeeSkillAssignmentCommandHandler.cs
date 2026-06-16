using Energy.Application.Organization.EmployeeSkillAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.EmployeeSkillAssignment.Commands.UpdateEmployeeSkillAssignment;

/// <summary>
/// <see cref="UpdateEmployeeSkillAssignmentCommand"/> handler'ı. <see cref="IEmployeeSkillAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateEmployeeSkillAssignmentCommandHandler
    : IRequestHandler<UpdateEmployeeSkillAssignmentCommand, BaseResponse<bool>>
{
    private readonly IEmployeeSkillAssignmentService _service;

    public UpdateEmployeeSkillAssignmentCommandHandler(IEmployeeSkillAssignmentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateEmployeeSkillAssignmentCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
