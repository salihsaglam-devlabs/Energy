using Energy.Application.Modules.Projects.ProjectNote.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectNote.Commands.DeleteProjectNote;

/// <summary>
/// <see cref="DeleteProjectNoteCommand"/> handler'ı. <see cref="IProjectNoteService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteProjectNoteCommandHandler
    : IRequestHandler<DeleteProjectNoteCommand, BaseResponse<bool>>
{
    private readonly IProjectNoteService _service;

    public DeleteProjectNoteCommandHandler(IProjectNoteService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteProjectNoteCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
