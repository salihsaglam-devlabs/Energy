using Energy.Application.Modules.Inventory.StockBalance.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockBalance.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockBalance.Queries.GetStockBalanceList;

/// <summary>
/// <see cref="GetStockBalanceListQuery"/> handler'ı. <see cref="IStockBalanceService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockBalanceListQueryHandler
    : IRequestHandler<GetStockBalanceListQuery, BaseResponse<PaginatedResponse<StockBalanceListResponse>>>
{
    private readonly IStockBalanceService _service;

    public GetStockBalanceListQueryHandler(IStockBalanceService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<StockBalanceListResponse>>> Handle(
        GetStockBalanceListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
