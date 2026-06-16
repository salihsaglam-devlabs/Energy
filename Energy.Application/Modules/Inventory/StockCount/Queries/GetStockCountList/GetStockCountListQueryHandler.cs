using Energy.Application.Modules.Inventory.StockCount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCount.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockCount.Queries.GetStockCountList;

/// <summary>
/// <see cref="GetStockCountListQuery"/> handler'ı. <see cref="IStockCountService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockCountListQueryHandler
    : IRequestHandler<GetStockCountListQuery, BaseResponse<PaginatedResponse<StockCountListResponse>>>
{
    private readonly IStockCountService _service;

    public GetStockCountListQueryHandler(IStockCountService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<StockCountListResponse>>> Handle(
        GetStockCountListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
