using Energy.Application.Documents.DocumentRelation.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentRelation.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentRelation.Queries.GetDocumentRelationLookup;

/// <summary>
/// <see cref="GetDocumentRelationLookupQuery"/> handler'ı. <see cref="IDocumentRelationLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentRelationLookupQueryHandler
    : IRequestHandler<GetDocumentRelationLookupQuery, BaseResponse<IReadOnlyList<DocumentRelationLookupResponse>>>
{
    private readonly IDocumentRelationLookupService _lookup;

    public GetDocumentRelationLookupQueryHandler(IDocumentRelationLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<DocumentRelationLookupResponse>>> Handle(
        GetDocumentRelationLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
