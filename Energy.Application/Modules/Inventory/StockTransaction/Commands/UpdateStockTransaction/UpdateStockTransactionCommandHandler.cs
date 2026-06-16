using Energy.Application.Modules.Inventory.StockTransaction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockTransaction.Commands.UpdateStockTransaction;

/// <summary>
/// <see cref="UpdateStockTransactionCommand"/> handler'ı. <see cref="IStockTransactionService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateStockTransactionCommandHandler
    : IRequestHandler<UpdateStockTransactionCommand, BaseResponse<bool>>
{
    private readonly IStockTransactionService _service;

    public UpdateStockTransactionCommandHandler(IStockTransactionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateStockTransactionCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
