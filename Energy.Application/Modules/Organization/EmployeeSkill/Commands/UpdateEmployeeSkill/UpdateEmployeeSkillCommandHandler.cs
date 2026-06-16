using Energy.Application.Modules.Organization.EmployeeSkill.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeeSkill.Commands.UpdateEmployeeSkill;

/// <summary>
/// <see cref="UpdateEmployeeSkillCommand"/> handler'ı. <see cref="IEmployeeSkillService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateEmployeeSkillCommandHandler
    : IRequestHandler<UpdateEmployeeSkillCommand, BaseResponse<bool>>
{
    private readonly IEmployeeSkillService _service;

    public UpdateEmployeeSkillCommandHandler(IEmployeeSkillService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateEmployeeSkillCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
