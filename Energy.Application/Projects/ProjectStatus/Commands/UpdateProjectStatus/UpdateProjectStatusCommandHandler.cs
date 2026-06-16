using Energy.Application.Projects.ProjectStatus.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectStatus.Commands.UpdateProjectStatus;

/// <summary>
/// <see cref="UpdateProjectStatusCommand"/> handler'ı. <see cref="IProjectStatusService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateProjectStatusCommandHandler
    : IRequestHandler<UpdateProjectStatusCommand, BaseResponse<bool>>
{
    private readonly IProjectStatusService _service;

    public UpdateProjectStatusCommandHandler(IProjectStatusService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateProjectStatusCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
