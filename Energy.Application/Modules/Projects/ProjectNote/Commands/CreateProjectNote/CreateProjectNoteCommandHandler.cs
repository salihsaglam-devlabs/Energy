using Energy.Application.Modules.Projects.ProjectNote.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectNote.Commands.CreateProjectNote;

/// <summary>
/// <see cref="CreateProjectNoteCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IProjectNoteService"/>'i orkestre eder.
/// </summary>
public sealed class CreateProjectNoteCommandHandler
    : IRequestHandler<CreateProjectNoteCommand, BaseResponse<Guid>>
{
    private readonly IProjectNoteService _service;

    public CreateProjectNoteCommandHandler(IProjectNoteService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateProjectNoteCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
