using Energy.Application.Modules.Inventory.StockDocumentLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocumentLine.Queries.GetStockDocumentLineLookup;

/// <summary>
/// <see cref="GetStockDocumentLineLookupQuery"/> handler'ı. <see cref="IStockDocumentLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockDocumentLineLookupQueryHandler
    : IRequestHandler<GetStockDocumentLineLookupQuery, BaseResponse<IReadOnlyList<StockDocumentLineLookupResponse>>>
{
    private readonly IStockDocumentLineLookupService _lookup;

    public GetStockDocumentLineLookupQueryHandler(IStockDocumentLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<StockDocumentLineLookupResponse>>> Handle(
        GetStockDocumentLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
