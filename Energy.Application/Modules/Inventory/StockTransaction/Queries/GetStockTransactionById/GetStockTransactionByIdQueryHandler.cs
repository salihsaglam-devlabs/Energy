using Energy.Application.Modules.Inventory.StockTransaction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockTransaction.Queries.GetStockTransactionById;

/// <summary>
/// <see cref="GetStockTransactionByIdQuery"/> handler'ı. <see cref="IStockTransactionService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockTransactionByIdQueryHandler
    : IRequestHandler<GetStockTransactionByIdQuery, BaseResponse<StockTransactionDetailResponse>>
{
    private readonly IStockTransactionService _service;

    public GetStockTransactionByIdQueryHandler(IStockTransactionService service)
        => _service = service;

    public Task<BaseResponse<StockTransactionDetailResponse>> Handle(
        GetStockTransactionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
