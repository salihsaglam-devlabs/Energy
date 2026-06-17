using Energy.Application.Documents.DocumentPermission.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Documents.DocumentPermission.Responses;
using MediatR;

namespace Energy.Application.Documents.DocumentPermission.Queries.GetDocumentPermissionLookup;

/// <summary>
/// <see cref="GetDocumentPermissionLookupQuery"/> handler'ı. <see cref="IDocumentPermissionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetDocumentPermissionLookupQueryHandler
    : IRequestHandler<GetDocumentPermissionLookupQuery, BaseResponse<IReadOnlyList<DocumentPermissionLookupResponse>>>
{
    private readonly IDocumentPermissionLookupService _lookup;

    public GetDocumentPermissionLookupQueryHandler(IDocumentPermissionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<DocumentPermissionLookupResponse>>> Handle(
        GetDocumentPermissionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
