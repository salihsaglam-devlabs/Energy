using Energy.Application.Modules.Organization.EmployeeSkill.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeeSkill.Commands.CreateEmployeeSkill;

/// <summary>
/// <see cref="CreateEmployeeSkillCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IEmployeeSkillService"/>'i orkestre eder.
/// </summary>
public sealed class CreateEmployeeSkillCommandHandler
    : IRequestHandler<CreateEmployeeSkillCommand, BaseResponse<Guid>>
{
    private readonly IEmployeeSkillService _service;

    public CreateEmployeeSkillCommandHandler(IEmployeeSkillService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateEmployeeSkillCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
