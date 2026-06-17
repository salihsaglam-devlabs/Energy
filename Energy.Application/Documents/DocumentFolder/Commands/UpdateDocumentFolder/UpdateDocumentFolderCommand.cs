using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Requests;
using MediatR;

namespace Energy.Application.Documents.DocumentFolder.Commands.UpdateDocumentFolder;

/// <summary>Var olan DocumentFolder kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateDocumentFolderCommand(Guid Id, UpdateDocumentFolderRequest Request)
    : IRequest<BaseResponse<bool>>;
