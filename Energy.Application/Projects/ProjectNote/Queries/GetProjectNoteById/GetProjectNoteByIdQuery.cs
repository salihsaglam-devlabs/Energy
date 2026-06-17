using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectNote.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectNote.Queries.GetProjectNoteById;

/// <summary>Kimliğe göre ProjectNote detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetProjectNoteByIdQuery(Guid Id)
    : IRequest<BaseResponse<ProjectNoteDetailResponse>>;
