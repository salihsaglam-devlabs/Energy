using Energy.Application.Modules.Documents.Files.Services;
using MediatR;

namespace Energy.Application.Modules.Documents.Files.Queries.GetDocumentVersionContent;

/// <summary><see cref="GetDocumentVersionContentQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetDocumentVersionContentQueryHandler
    : IRequestHandler<GetDocumentVersionContentQuery, DocumentDownload?>
{
    private readonly IDocumentFileService _files;

    public GetDocumentVersionContentQueryHandler(IDocumentFileService files)
        => _files = files;

    public Task<DocumentDownload?> Handle(GetDocumentVersionContentQuery request, CancellationToken ct)
        => _files.GetVersionContentAsync(request.VersionId, ct);
}
