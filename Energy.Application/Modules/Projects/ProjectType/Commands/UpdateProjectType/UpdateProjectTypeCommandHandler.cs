using Energy.Application.Modules.Projects.ProjectType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectType.Commands.UpdateProjectType;

/// <summary>
/// <see cref="UpdateProjectTypeCommand"/> handler'ı. <see cref="IProjectTypeService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateProjectTypeCommandHandler
    : IRequestHandler<UpdateProjectTypeCommand, BaseResponse<bool>>
{
    private readonly IProjectTypeService _service;

    public UpdateProjectTypeCommandHandler(IProjectTypeService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateProjectTypeCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
