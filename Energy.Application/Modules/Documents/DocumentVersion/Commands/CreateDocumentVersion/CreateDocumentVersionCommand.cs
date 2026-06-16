using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Requests;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentVersion.Commands.CreateDocumentVersion;

/// <summary>Yeni DocumentVersion oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateDocumentVersionCommand(CreateDocumentVersionRequest Request)
    : IRequest<BaseResponse<Guid>>;
