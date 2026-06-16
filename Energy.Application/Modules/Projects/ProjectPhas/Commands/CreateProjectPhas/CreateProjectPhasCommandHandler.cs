using Energy.Application.Modules.Projects.ProjectPhas.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectPhas.Commands.CreateProjectPhas;

/// <summary>
/// <see cref="CreateProjectPhasCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IProjectPhasService"/>'i orkestre eder.
/// </summary>
public sealed class CreateProjectPhasCommandHandler
    : IRequestHandler<CreateProjectPhasCommand, BaseResponse<Guid>>
{
    private readonly IProjectPhasService _service;

    public CreateProjectPhasCommandHandler(IProjectPhasService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateProjectPhasCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
