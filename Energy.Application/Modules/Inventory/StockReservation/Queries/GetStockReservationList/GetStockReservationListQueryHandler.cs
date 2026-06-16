using Energy.Application.Modules.Inventory.StockReservation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockReservation.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockReservation.Queries.GetStockReservationList;

/// <summary>
/// <see cref="GetStockReservationListQuery"/> handler'ı. <see cref="IStockReservationService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockReservationListQueryHandler
    : IRequestHandler<GetStockReservationListQuery, BaseResponse<PaginatedResponse<StockReservationListResponse>>>
{
    private readonly IStockReservationService _service;

    public GetStockReservationListQueryHandler(IStockReservationService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<StockReservationListResponse>>> Handle(
        GetStockReservationListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
