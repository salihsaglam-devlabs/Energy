using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Projects.ProjectNote.Commands.DeleteProjectNote;

/// <summary>ProjectNote kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteProjectNoteCommand(Guid Id) : IRequest<BaseResponse<bool>>;
