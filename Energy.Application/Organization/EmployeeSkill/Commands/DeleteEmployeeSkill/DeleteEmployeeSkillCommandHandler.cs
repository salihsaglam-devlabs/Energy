using Energy.Application.Organization.EmployeeSkill.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.EmployeeSkill.Commands.DeleteEmployeeSkill;

/// <summary>
/// <see cref="DeleteEmployeeSkillCommand"/> handler'ı. <see cref="IEmployeeSkillService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteEmployeeSkillCommandHandler
    : IRequestHandler<DeleteEmployeeSkillCommand, BaseResponse<bool>>
{
    private readonly IEmployeeSkillService _service;

    public DeleteEmployeeSkillCommandHandler(IEmployeeSkillService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteEmployeeSkillCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
