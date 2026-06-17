using Energy.Application.Organization.EmployeeSkillAssignment.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.EmployeeSkillAssignment.Commands.CreateEmployeeSkillAssignment;

/// <summary>
/// <see cref="CreateEmployeeSkillAssignmentCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IEmployeeSkillAssignmentService"/>'i orkestre eder.
/// </summary>
public sealed class CreateEmployeeSkillAssignmentCommandHandler
    : IRequestHandler<CreateEmployeeSkillAssignmentCommand, BaseResponse<Guid>>
{
    private readonly IEmployeeSkillAssignmentService _service;

    public CreateEmployeeSkillAssignmentCommandHandler(IEmployeeSkillAssignmentService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateEmployeeSkillAssignmentCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
