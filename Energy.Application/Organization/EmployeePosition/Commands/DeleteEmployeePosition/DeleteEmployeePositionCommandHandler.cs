using Energy.Application.Organization.EmployeePosition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.EmployeePosition.Commands.DeleteEmployeePosition;

/// <summary>
/// <see cref="DeleteEmployeePositionCommand"/> handler'ı. <see cref="IEmployeePositionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteEmployeePositionCommandHandler
    : IRequestHandler<DeleteEmployeePositionCommand, BaseResponse<bool>>
{
    private readonly IEmployeePositionService _service;

    public DeleteEmployeePositionCommandHandler(IEmployeePositionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteEmployeePositionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
