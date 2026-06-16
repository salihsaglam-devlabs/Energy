using Energy.Application.Modules.Projects.ProjectType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectType.Commands.DeleteProjectType;

/// <summary>
/// <see cref="DeleteProjectTypeCommand"/> handler'ı. <see cref="IProjectTypeService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteProjectTypeCommandHandler
    : IRequestHandler<DeleteProjectTypeCommand, BaseResponse<bool>>
{
    private readonly IProjectTypeService _service;

    public DeleteProjectTypeCommandHandler(IProjectTypeService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteProjectTypeCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
