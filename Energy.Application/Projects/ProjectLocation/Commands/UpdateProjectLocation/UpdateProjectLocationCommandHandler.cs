using Energy.Application.Projects.ProjectLocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectLocation.Commands.UpdateProjectLocation;

/// <summary>
/// <see cref="UpdateProjectLocationCommand"/> handler'ı. <see cref="IProjectLocationService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateProjectLocationCommandHandler
    : IRequestHandler<UpdateProjectLocationCommand, BaseResponse<bool>>
{
    private readonly IProjectLocationService _service;

    public UpdateProjectLocationCommandHandler(IProjectLocationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateProjectLocationCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
