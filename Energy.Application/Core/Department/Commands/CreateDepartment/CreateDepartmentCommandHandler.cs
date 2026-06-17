using Energy.Application.Core.Department.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.Department.Commands.CreateDepartment;

/// <summary>
/// <see cref="CreateDepartmentCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IDepartmentService"/>'i orkestre eder.
/// </summary>
public sealed class CreateDepartmentCommandHandler
    : IRequestHandler<CreateDepartmentCommand, BaseResponse<Guid>>
{
    private readonly IDepartmentService _service;

    public CreateDepartmentCommandHandler(IDepartmentService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateDepartmentCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
