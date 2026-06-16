using Energy.Application.Modules.Projects.ProjectNote.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectNote.Responses;
using MediatR;

namespace Energy.Application.Modules.Projects.ProjectNote.Queries.GetProjectNoteById;

/// <summary>
/// <see cref="GetProjectNoteByIdQuery"/> handler'ı. <see cref="IProjectNoteService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectNoteByIdQueryHandler
    : IRequestHandler<GetProjectNoteByIdQuery, BaseResponse<ProjectNoteDetailResponse>>
{
    private readonly IProjectNoteService _service;

    public GetProjectNoteByIdQueryHandler(IProjectNoteService service)
        => _service = service;

    public Task<BaseResponse<ProjectNoteDetailResponse>> Handle(
        GetProjectNoteByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
