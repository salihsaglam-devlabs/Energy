using Energy.Application.Modules.Documents.DocumentFolder.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentFolder.Responses;
using MediatR;

namespace Energy.Application.Modules.Documents.DocumentFolder.Queries.GetDocumentFolderLookup;

/// <summary>
/// <see cref="GetDocumentFolderLookupQuery"/> handler'ı. <see cref="IDocumentFolderLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentFolderLookupQueryHandler
    : IRequestHandler<GetDocumentFolderLookupQuery, BaseResponse<IReadOnlyList<DocumentFolderLookupResponse>>>
{
    private readonly IDocumentFolderLookupService _lookup;

    public GetDocumentFolderLookupQueryHandler(IDocumentFolderLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<DocumentFolderLookupResponse>>> Handle(
        GetDocumentFolderLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
