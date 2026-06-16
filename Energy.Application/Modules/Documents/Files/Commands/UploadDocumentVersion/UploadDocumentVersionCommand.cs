using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.Files.Commands.UploadDocumentVersion;

/// <summary>Belgeye yeni bir dosya versiyonu yükleme use-case'i.</summary>
public sealed record UploadDocumentVersionCommand(
    Guid DocumentId, byte[] Content, string FileName, string ContentType, long Length)
    : IRequest<BaseResponse<DocumentVersionFileResponse>>;
