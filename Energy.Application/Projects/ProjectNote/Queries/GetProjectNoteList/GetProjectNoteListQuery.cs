using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectNote.Requests;
using Energy.Shared.Models.V1.Projects.ProjectNote.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectNote.Queries.GetProjectNoteList;

/// <summary>Sayfalanmış ProjectNote listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetProjectNoteListQuery(GetProjectNoteListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ProjectNoteListResponse>>>;
