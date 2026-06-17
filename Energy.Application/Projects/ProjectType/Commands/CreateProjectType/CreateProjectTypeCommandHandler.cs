using Energy.Application.Projects.ProjectType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectType.Commands.CreateProjectType;

/// <summary>
/// <see cref="CreateProjectTypeCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IProjectTypeService"/>'i orkestre eder.
/// </summary>
public sealed class CreateProjectTypeCommandHandler
    : IRequestHandler<CreateProjectTypeCommand, BaseResponse<Guid>>
{
    private readonly IProjectTypeService _service;

    public CreateProjectTypeCommandHandler(IProjectTypeService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateProjectTypeCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
