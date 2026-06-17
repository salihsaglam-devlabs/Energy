using Energy.Application.Inventory.StockCountLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockCountLine.Queries.GetStockCountLineList;

/// <summary>
/// <see cref="GetStockCountLineListQuery"/> handler'ı. <see cref="IStockCountLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockCountLineListQueryHandler
    : IRequestHandler<GetStockCountLineListQuery, BaseResponse<PaginatedResponse<StockCountLineListResponse>>>
{
    private readonly IStockCountLineService _service;

    public GetStockCountLineListQueryHandler(IStockCountLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<StockCountLineListResponse>>> Handle(
        GetStockCountLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
