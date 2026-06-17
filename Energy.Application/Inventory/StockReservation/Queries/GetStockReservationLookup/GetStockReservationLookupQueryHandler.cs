using Energy.Application.Inventory.StockReservation.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockReservation.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockReservation.Queries.GetStockReservationLookup;

/// <summary>
/// <see cref="GetStockReservationLookupQuery"/> handler'ı. <see cref="IStockReservationLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockReservationLookupQueryHandler
    : IRequestHandler<GetStockReservationLookupQuery, BaseResponse<IReadOnlyList<StockReservationLookupResponse>>>
{
    private readonly IStockReservationLookupService _lookup;

    public GetStockReservationLookupQueryHandler(IStockReservationLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<StockReservationLookupResponse>>> Handle(
        GetStockReservationLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
