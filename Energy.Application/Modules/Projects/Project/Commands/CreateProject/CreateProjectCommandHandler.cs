using Energy.Application.Modules.Projects.Project.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.Project.Commands.CreateProject;

/// <summary>
/// <see cref="CreateProjectCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IProjectService"/>'i orkestre eder.
/// </summary>
public sealed class CreateProjectCommandHandler
    : IRequestHandler<CreateProjectCommand, BaseResponse<Guid>>
{
    private readonly IProjectService _service;

    public CreateProjectCommandHandler(IProjectService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateProjectCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
