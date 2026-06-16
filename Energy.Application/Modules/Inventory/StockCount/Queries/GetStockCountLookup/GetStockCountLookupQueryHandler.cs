using Energy.Application.Modules.Inventory.StockCount.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCount.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockCount.Queries.GetStockCountLookup;

/// <summary>
/// <see cref="GetStockCountLookupQuery"/> handler'ı. <see cref="IStockCountLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockCountLookupQueryHandler
    : IRequestHandler<GetStockCountLookupQuery, BaseResponse<IReadOnlyList<StockCountLookupResponse>>>
{
    private readonly IStockCountLookupService _lookup;

    public GetStockCountLookupQueryHandler(IStockCountLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<StockCountLookupResponse>>> Handle(
        GetStockCountLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
