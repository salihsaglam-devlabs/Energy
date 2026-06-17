using Energy.Application.Documents.DocumentVersion.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentVersion.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentVersion.Queries.GetDocumentVersionLookup;

/// <summary>
/// <see cref="GetDocumentVersionLookupQuery"/> handler'ı. <see cref="IDocumentVersionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentVersionLookupQueryHandler
    : IRequestHandler<GetDocumentVersionLookupQuery, BaseResponse<IReadOnlyList<DocumentVersionLookupResponse>>>
{
    private readonly IDocumentVersionLookupService _lookup;

    public GetDocumentVersionLookupQueryHandler(IDocumentVersionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<DocumentVersionLookupResponse>>> Handle(
        GetDocumentVersionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
