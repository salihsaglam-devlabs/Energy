using Energy.Application.Core.Department.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Core.Department.Commands.DeleteDepartment;

/// <summary>
/// <see cref="DeleteDepartmentCommand"/> handler'ı. <see cref="IDepartmentService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteDepartmentCommandHandler
    : IRequestHandler<DeleteDepartmentCommand, BaseResponse<bool>>
{
    private readonly IDepartmentService _service;

    public DeleteDepartmentCommandHandler(IDepartmentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteDepartmentCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
