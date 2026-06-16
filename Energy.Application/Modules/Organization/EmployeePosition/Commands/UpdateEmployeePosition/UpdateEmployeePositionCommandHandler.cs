using Energy.Application.Modules.Organization.EmployeePosition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.EmployeePosition.Commands.UpdateEmployeePosition;

/// <summary>
/// <see cref="UpdateEmployeePositionCommand"/> handler'ı. <see cref="IEmployeePositionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateEmployeePositionCommandHandler
    : IRequestHandler<UpdateEmployeePositionCommand, BaseResponse<bool>>
{
    private readonly IEmployeePositionService _service;

    public UpdateEmployeePositionCommandHandler(IEmployeePositionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateEmployeePositionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
