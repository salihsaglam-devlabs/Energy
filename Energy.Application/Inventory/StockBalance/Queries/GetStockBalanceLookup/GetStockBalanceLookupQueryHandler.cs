using Energy.Application.Inventory.StockBalance.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockBalance.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockBalance.Queries.GetStockBalanceLookup;

/// <summary>
/// <see cref="GetStockBalanceLookupQuery"/> handler'ı. <see cref="IStockBalanceLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockBalanceLookupQueryHandler
    : IRequestHandler<GetStockBalanceLookupQuery, BaseResponse<IReadOnlyList<StockBalanceLookupResponse>>>
{
    private readonly IStockBalanceLookupService _lookup;

    public GetStockBalanceLookupQueryHandler(IStockBalanceLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<StockBalanceLookupResponse>>> Handle(
        GetStockBalanceLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
