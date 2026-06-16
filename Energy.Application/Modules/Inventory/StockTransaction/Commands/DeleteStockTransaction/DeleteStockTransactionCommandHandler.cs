using Energy.Application.Modules.Inventory.StockTransaction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockTransaction.Commands.DeleteStockTransaction;

/// <summary>
/// <see cref="DeleteStockTransactionCommand"/> handler'ı. <see cref="IStockTransactionService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteStockTransactionCommandHandler
    : IRequestHandler<DeleteStockTransactionCommand, BaseResponse<bool>>
{
    private readonly IStockTransactionService _service;

    public DeleteStockTransactionCommandHandler(IStockTransactionService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteStockTransactionCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
