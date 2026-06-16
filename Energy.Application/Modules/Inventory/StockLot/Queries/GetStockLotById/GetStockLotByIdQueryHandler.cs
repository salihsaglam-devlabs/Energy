using Energy.Application.Modules.Inventory.StockLot.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockLot.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockLot.Queries.GetStockLotById;

/// <summary>
/// <see cref="GetStockLotByIdQuery"/> handler'ı. <see cref="IStockLotService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockLotByIdQueryHandler
    : IRequestHandler<GetStockLotByIdQuery, BaseResponse<StockLotDetailResponse>>
{
    private readonly IStockLotService _service;

    public GetStockLotByIdQueryHandler(IStockLotService service)
        => _service = service;

    public Task<BaseResponse<StockLotDetailResponse>> Handle(
        GetStockLotByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
