using Energy.Application.Modules.Projects.ProjectNote.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectNote.Commands.UpdateProjectNote;

/// <summary>
/// <see cref="UpdateProjectNoteCommand"/> handler'ı. <see cref="IProjectNoteService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateProjectNoteCommandHandler
    : IRequestHandler<UpdateProjectNoteCommand, BaseResponse<bool>>
{
    private readonly IProjectNoteService _service;

    public UpdateProjectNoteCommandHandler(IProjectNoteService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateProjectNoteCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
