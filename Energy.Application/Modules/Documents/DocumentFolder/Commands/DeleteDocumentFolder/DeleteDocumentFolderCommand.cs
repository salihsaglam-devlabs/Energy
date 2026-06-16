using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentFolder.Commands.DeleteDocumentFolder;

/// <summary>DocumentFolder kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteDocumentFolderCommand(Guid Id) : IRequest<BaseResponse<bool>>;
