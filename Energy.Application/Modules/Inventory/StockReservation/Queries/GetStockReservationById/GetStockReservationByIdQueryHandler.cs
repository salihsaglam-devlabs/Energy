using Energy.Application.Modules.Inventory.StockReservation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockReservation.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockReservation.Queries.GetStockReservationById;

/// <summary>
/// <see cref="GetStockReservationByIdQuery"/> handler'ı. <see cref="IStockReservationService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockReservationByIdQueryHandler
    : IRequestHandler<GetStockReservationByIdQuery, BaseResponse<StockReservationDetailResponse>>
{
    private readonly IStockReservationService _service;

    public GetStockReservationByIdQueryHandler(IStockReservationService service)
        => _service = service;

    public Task<BaseResponse<StockReservationDetailResponse>> Handle(
        GetStockReservationByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
