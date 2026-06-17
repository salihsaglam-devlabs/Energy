using Energy.Application.Projects.ProjectPhas.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectPhas.Commands.UpdateProjectPhas;

/// <summary>
/// <see cref="UpdateProjectPhasCommand"/> handler'ı. <see cref="IProjectPhasService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateProjectPhasCommandHandler
    : IRequestHandler<UpdateProjectPhasCommand, BaseResponse<bool>>
{
    private readonly IProjectPhasService _service;

    public UpdateProjectPhasCommandHandler(IProjectPhasService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateProjectPhasCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
