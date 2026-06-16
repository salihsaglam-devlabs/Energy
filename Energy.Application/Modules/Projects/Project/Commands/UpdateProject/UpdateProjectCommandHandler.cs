using Energy.Application.Modules.Projects.Project.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.Project.Commands.UpdateProject;

/// <summary>
/// <see cref="UpdateProjectCommand"/> handler'ı. <see cref="IProjectService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateProjectCommandHandler
    : IRequestHandler<UpdateProjectCommand, BaseResponse<bool>>
{
    private readonly IProjectService _service;

    public UpdateProjectCommandHandler(IProjectService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateProjectCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
