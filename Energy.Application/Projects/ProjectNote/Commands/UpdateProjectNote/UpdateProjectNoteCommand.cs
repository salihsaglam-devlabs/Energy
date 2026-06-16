using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectNote.Requests;
using MediatR;

namespace Energy.Application.Projects.ProjectNote.Commands.UpdateProjectNote;

/// <summary>Var olan ProjectNote kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateProjectNoteCommand(Guid Id, UpdateProjectNoteRequest Request)
    : IRequest<BaseResponse<bool>>;
