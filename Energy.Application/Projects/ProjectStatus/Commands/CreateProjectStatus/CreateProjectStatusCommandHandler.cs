using Energy.Application.Projects.ProjectStatus.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectStatus.Commands.CreateProjectStatus;

/// <summary>
/// <see cref="CreateProjectStatusCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IProjectStatusService"/>'i orkestre eder.
/// </summary>
public sealed class CreateProjectStatusCommandHandler
    : IRequestHandler<CreateProjectStatusCommand, BaseResponse<Guid>>
{
    private readonly IProjectStatusService _service;

    public CreateProjectStatusCommandHandler(IProjectStatusService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateProjectStatusCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
