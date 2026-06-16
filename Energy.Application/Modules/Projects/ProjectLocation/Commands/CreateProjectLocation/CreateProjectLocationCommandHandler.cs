using Energy.Application.Modules.Projects.ProjectLocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectLocation.Commands.CreateProjectLocation;

/// <summary>
/// <see cref="CreateProjectLocationCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IProjectLocationService"/>'i orkestre eder.
/// </summary>
public sealed class CreateProjectLocationCommandHandler
    : IRequestHandler<CreateProjectLocationCommand, BaseResponse<Guid>>
{
    private readonly IProjectLocationService _service;

    public CreateProjectLocationCommandHandler(IProjectLocationService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateProjectLocationCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
