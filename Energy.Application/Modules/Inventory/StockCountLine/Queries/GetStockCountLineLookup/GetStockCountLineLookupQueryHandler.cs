using Energy.Application.Modules.Inventory.StockCountLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockCountLine.Queries.GetStockCountLineLookup;

/// <summary>
/// <see cref="GetStockCountLineLookupQuery"/> handler'ı. <see cref="IStockCountLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockCountLineLookupQueryHandler
    : IRequestHandler<GetStockCountLineLookupQuery, BaseResponse<IReadOnlyList<StockCountLineLookupResponse>>>
{
    private readonly IStockCountLineLookupService _lookup;

    public GetStockCountLineLookupQueryHandler(IStockCountLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<StockCountLineLookupResponse>>> Handle(
        GetStockCountLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
