using Energy.Application.Modules.Organization.EmployeeSkillAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeeSkillAssignment.Commands.DeleteEmployeeSkillAssignment;

/// <summary>
/// <see cref="DeleteEmployeeSkillAssignmentCommand"/> handler'ı. <see cref="IEmployeeSkillAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteEmployeeSkillAssignmentCommandHandler
    : IRequestHandler<DeleteEmployeeSkillAssignmentCommand, BaseResponse<bool>>
{
    private readonly IEmployeeSkillAssignmentService _service;

    public DeleteEmployeeSkillAssignmentCommandHandler(IEmployeeSkillAssignmentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteEmployeeSkillAssignmentCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
