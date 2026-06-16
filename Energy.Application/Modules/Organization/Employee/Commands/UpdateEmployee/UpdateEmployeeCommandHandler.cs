using Energy.Application.Modules.Organization.Employee.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.Employee.Commands.UpdateEmployee;

/// <summary>
/// <see cref="UpdateEmployeeCommand"/> handler'ı. <see cref="IEmployeeService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateEmployeeCommandHandler
    : IRequestHandler<UpdateEmployeeCommand, BaseResponse<bool>>
{
    private readonly IEmployeeService _service;

    public UpdateEmployeeCommandHandler(IEmployeeService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
