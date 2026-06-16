using Energy.Application.Inventory.StockTransaction.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockTransaction.Commands.CreateStockTransaction;

/// <summary>
/// <see cref="CreateStockTransactionCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IStockTransactionService"/>'i orkestre eder.
/// </summary>
public sealed class CreateStockTransactionCommandHandler
    : IRequestHandler<CreateStockTransactionCommand, BaseResponse<Guid>>
{
    private readonly IStockTransactionService _service;

    public CreateStockTransactionCommandHandler(IStockTransactionService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateStockTransactionCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
