using Energy.Application.Inventory.StockLot.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockLot.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockLot.Queries.GetStockLotLookup;

/// <summary>
/// <see cref="GetStockLotLookupQuery"/> handler'ı. <see cref="IStockLotLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockLotLookupQueryHandler
    : IRequestHandler<GetStockLotLookupQuery, BaseResponse<IReadOnlyList<StockLotLookupResponse>>>
{
    private readonly IStockLotLookupService _lookup;

    public GetStockLotLookupQueryHandler(IStockLotLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<StockLotLookupResponse>>> Handle(
        GetStockLotLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
