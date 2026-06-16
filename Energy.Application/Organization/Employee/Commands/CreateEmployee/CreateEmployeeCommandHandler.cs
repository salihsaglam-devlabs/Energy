using Energy.Application.Organization.Employee.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.Employee.Commands.CreateEmployee;

/// <summary>
/// <see cref="CreateEmployeeCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IEmployeeService"/>'i orkestre eder.
/// </summary>
public sealed class CreateEmployeeCommandHandler
    : IRequestHandler<CreateEmployeeCommand, BaseResponse<Guid>>
{
    private readonly IEmployeeService _service;

    public CreateEmployeeCommandHandler(IEmployeeService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
