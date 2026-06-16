using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentVersion.Commands.DeleteDocumentVersion;

/// <summary>DocumentVersion kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteDocumentVersionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
