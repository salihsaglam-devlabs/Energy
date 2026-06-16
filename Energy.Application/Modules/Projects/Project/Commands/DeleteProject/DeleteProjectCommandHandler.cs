using Energy.Application.Modules.Projects.Project.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.Project.Commands.DeleteProject;

/// <summary>
/// <see cref="DeleteProjectCommand"/> handler'ı. <see cref="IProjectService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteProjectCommandHandler
    : IRequestHandler<DeleteProjectCommand, BaseResponse<bool>>
{
    private readonly IProjectService _service;

    public DeleteProjectCommandHandler(IProjectService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteProjectCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
