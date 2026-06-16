using Energy.Application.Organization.Employee.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.Employee.Commands.DeleteEmployee;

/// <summary>
/// <see cref="DeleteEmployeeCommand"/> handler'ı. <see cref="IEmployeeService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteEmployeeCommandHandler
    : IRequestHandler<DeleteEmployeeCommand, BaseResponse<bool>>
{
    private readonly IEmployeeService _service;

    public DeleteEmployeeCommandHandler(IEmployeeService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteEmployeeCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
