using Energy.Application.Projects.ProjectNote.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectNote.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectNote.Queries.GetProjectNoteList;

/// <summary>
/// <see cref="GetProjectNoteListQuery"/> handler'ı. <see cref="IProjectNoteService"/>'i orkestre eder.
/// </summary>
public sealed class GetProjectNoteListQueryHandler
    : IRequestHandler<GetProjectNoteListQuery, BaseResponse<PaginatedResponse<ProjectNoteListResponse>>>
{
    private readonly IProjectNoteService _service;

    public GetProjectNoteListQueryHandler(IProjectNoteService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ProjectNoteListResponse>>> Handle(
        GetProjectNoteListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
