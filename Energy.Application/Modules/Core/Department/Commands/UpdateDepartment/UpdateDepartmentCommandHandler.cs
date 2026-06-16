using Energy.Application.Modules.Core.Department.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Core.Department.Commands.UpdateDepartment;

/// <summary>
/// <see cref="UpdateDepartmentCommand"/> handler'ı. <see cref="IDepartmentService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateDepartmentCommandHandler
    : IRequestHandler<UpdateDepartmentCommand, BaseResponse<bool>>
{
    private readonly IDepartmentService _service;

    public UpdateDepartmentCommandHandler(IDepartmentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateDepartmentCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
