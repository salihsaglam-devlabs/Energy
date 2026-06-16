using Energy.Application.Inventory.StockTransaction.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockTransaction.Queries.GetStockTransactionLookup;

/// <summary>
/// <see cref="GetStockTransactionLookupQuery"/> handler'ı. <see cref="IStockTransactionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockTransactionLookupQueryHandler
    : IRequestHandler<GetStockTransactionLookupQuery, BaseResponse<IReadOnlyList<StockTransactionLookupResponse>>>
{
    private readonly IStockTransactionLookupService _lookup;

    public GetStockTransactionLookupQueryHandler(IStockTransactionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<StockTransactionLookupResponse>>> Handle(
        GetStockTransactionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
