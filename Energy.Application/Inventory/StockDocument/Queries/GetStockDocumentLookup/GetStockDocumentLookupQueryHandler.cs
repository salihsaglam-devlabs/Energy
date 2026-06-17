using Energy.Application.Inventory.StockDocument.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocument.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocument.Queries.GetStockDocumentLookup;

/// <summary>
/// <see cref="GetStockDocumentLookupQuery"/> handler'ı. <see cref="IStockDocumentLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockDocumentLookupQueryHandler
    : IRequestHandler<GetStockDocumentLookupQuery, BaseResponse<IReadOnlyList<StockDocumentLookupResponse>>>
{
    private readonly IStockDocumentLookupService _lookup;

    public GetStockDocumentLookupQueryHandler(IStockDocumentLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<StockDocumentLookupResponse>>> Handle(
        GetStockDocumentLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
