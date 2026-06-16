using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.ProjectNote.Requests;
using MediatR;

namespace Energy.Application.Projects.ProjectNote.Commands.CreateProjectNote;

/// <summary>Yeni ProjectNote oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateProjectNoteCommand(CreateProjectNoteRequest Request)
    : IRequest<BaseResponse<Guid>>;
