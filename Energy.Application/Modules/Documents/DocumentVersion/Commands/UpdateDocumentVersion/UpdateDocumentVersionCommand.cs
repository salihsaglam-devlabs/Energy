using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Requests;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentVersion.Commands.UpdateDocumentVersion;

/// <summary>Var olan DocumentVersion kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateDocumentVersionCommand(Guid Id, UpdateDocumentVersionRequest Request)
    : IRequest<BaseResponse<bool>>;
