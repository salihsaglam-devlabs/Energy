using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Requests;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentFolder.Commands.CreateDocumentFolder;

/// <summary>Yeni DocumentFolder oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateDocumentFolderCommand(CreateDocumentFolderRequest Request)
    : IRequest<BaseResponse<Guid>>;
