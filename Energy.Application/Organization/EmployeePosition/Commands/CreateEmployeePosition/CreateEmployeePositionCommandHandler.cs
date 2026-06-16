using Energy.Application.Organization.EmployeePosition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.EmployeePosition.Commands.CreateEmployeePosition;

/// <summary>
/// <see cref="CreateEmployeePositionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IEmployeePositionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateEmployeePositionCommandHandler
    : IRequestHandler<CreateEmployeePositionCommand, BaseResponse<Guid>>
{
    private readonly IEmployeePositionService _service;

    public CreateEmployeePositionCommandHandler(IEmployeePositionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateEmployeePositionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
