using Energy.Application.Documents.Document.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.Document.Responses;
using MediatR;

namespace Energy.Application.Documents.Document.Queries.GetDocumentLookup;

/// <summary>
/// <see cref="GetDocumentLookupQuery"/> handler'ı. <see cref="IDocumentLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentLookupQueryHandler
    : IRequestHandler<GetDocumentLookupQuery, BaseResponse<IReadOnlyList<DocumentLookupResponse>>>
{
    private readonly IDocumentLookupService _lookup;

    public GetDocumentLookupQueryHandler(IDocumentLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<DocumentLookupResponse>>> Handle(
        GetDocumentLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
