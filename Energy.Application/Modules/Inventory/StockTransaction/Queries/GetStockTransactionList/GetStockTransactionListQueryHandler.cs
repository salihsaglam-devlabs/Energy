using Energy.Application.Modules.Inventory.StockTransaction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockTransaction.Queries.GetStockTransactionList;

/// <summary>
/// <see cref="GetStockTransactionListQuery"/> handler'ı. <see cref="IStockTransactionService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockTransactionListQueryHandler
    : IRequestHandler<GetStockTransactionListQuery, BaseResponse<PaginatedResponse<StockTransactionListResponse>>>
{
    private readonly IStockTransactionService _service;

    public GetStockTransactionListQueryHandler(IStockTransactionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<StockTransactionListResponse>>> Handle(
        GetStockTransactionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
