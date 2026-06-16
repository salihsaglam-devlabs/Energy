using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Document.Requests;
using MediatR;

namespace Energy.Application.Modules.Documents.Document.Commands.CreateDocument;

/// <summary>Yeni Document oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateDocumentCommand(CreateDocumentRequest Request)
    : IRequest<BaseResponse<Guid>>;
