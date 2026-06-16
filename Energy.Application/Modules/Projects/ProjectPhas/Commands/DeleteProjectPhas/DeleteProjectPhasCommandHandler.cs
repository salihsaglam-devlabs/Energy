using Energy.Application.Modules.Projects.ProjectPhas.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectPhas.Commands.DeleteProjectPhas;

/// <summary>
/// <see cref="DeleteProjectPhasCommand"/> handler'ı. <see cref="IProjectPhasService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteProjectPhasCommandHandler
    : IRequestHandler<DeleteProjectPhasCommand, BaseResponse<bool>>
{
    private readonly IProjectPhasService _service;

    public DeleteProjectPhasCommandHandler(IProjectPhasService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteProjectPhasCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
