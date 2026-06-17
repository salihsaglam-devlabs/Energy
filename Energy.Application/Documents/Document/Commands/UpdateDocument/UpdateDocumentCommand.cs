using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Document.Requests;
using MediatR;

namespace Energy.Application.Documents.Document.Commands.UpdateDocument;

/// <summary>Var olan Document kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateDocumentCommand(Guid Id, UpdateDocumentRequest Request)
    : IRequest<BaseResponse<bool>>;
