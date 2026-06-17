using Energy.Application.Documents.Files.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;
using MediatR;

namespace Energy.Application.Documents.Files.Commands.UploadDocumentVersion;

/// <summary><see cref="UploadDocumentVersionCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class UploadDocumentVersionCommandHandler
    : IRequestHandler<UploadDocumentVersionCommand, BaseResponse<DocumentVersionFileResponse>>
{
    private readonly IDocumentFileService _files;

    public UploadDocumentVersionCommandHandler(IDocumentFileService files)
        => _files = files;

    public async Task<BaseResponse<DocumentVersionFileResponse>> Handle(
        UploadDocumentVersionCommand request, CancellationToken ct)
    {
        using var stream = new MemoryStream(request.Content);
        return await _files.UploadNewVersionAsync(
            request.DocumentId, stream, request.FileName, request.ContentType, request.Length, ct);
    }
}
