using Energy.Application.Modules.Projects.ProjectStatus.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectStatus.Commands.DeleteProjectStatus;

/// <summary>
/// <see cref="DeleteProjectStatusCommand"/> handler'ı. <see cref="IProjectStatusService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteProjectStatusCommandHandler
    : IRequestHandler<DeleteProjectStatusCommand, BaseResponse<bool>>
{
    private readonly IProjectStatusService _service;

    public DeleteProjectStatusCommandHandler(IProjectStatusService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteProjectStatusCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
