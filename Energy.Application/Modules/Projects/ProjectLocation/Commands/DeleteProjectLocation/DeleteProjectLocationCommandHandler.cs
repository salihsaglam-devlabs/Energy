using Energy.Application.Modules.Projects.ProjectLocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectLocation.Commands.DeleteProjectLocation;

/// <summary>
/// <see cref="DeleteProjectLocationCommand"/> handler'ı. <see cref="IProjectLocationService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteProjectLocationCommandHandler
    : IRequestHandler<DeleteProjectLocationCommand, BaseResponse<bool>>
{
    private readonly IProjectLocationService _service;

    public DeleteProjectLocationCommandHandler(IProjectLocationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteProjectLocationCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
