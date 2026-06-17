using Energy.Application.Documents.Files.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Files.Responses;
using MediatR;

namespace Energy.Application.Documents.Files.Queries.GetDocumentVersions;

/// <summary><see cref="GetDocumentVersionsQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetDocumentVersionsQueryHandler
    : IRequestHandler<GetDocumentVersionsQuery, BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>>
{
    private readonly IDocumentFileService _files;

    public GetDocumentVersionsQueryHandler(IDocumentFileService files)
        => _files = files;

    public Task<BaseResponse<IReadOnlyList<DocumentVersionFileResponse>>> Handle(
        GetDocumentVersionsQuery request, CancellationToken ct)
        => _files.GetVersionsAsync(request.DocumentId, ct);
}
