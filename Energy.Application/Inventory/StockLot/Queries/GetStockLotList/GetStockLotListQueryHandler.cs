using Energy.Application.Inventory.StockLot.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockLot.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockLot.Queries.GetStockLotList;

/// <summary>
/// <see cref="GetStockLotListQuery"/> handler'ı. <see cref="IStockLotService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockLotListQueryHandler
    : IRequestHandler<GetStockLotListQuery, BaseResponse<PaginatedResponse<StockLotListResponse>>>
{
    private readonly IStockLotService _service;

    public GetStockLotListQueryHandler(IStockLotService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<StockLotListResponse>>> Handle(
        GetStockLotListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
