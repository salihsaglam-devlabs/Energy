using Energy.Application.Inventory.StockBalance.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockBalance.Commands.DeleteStockBalance;

/// <summary>
/// <see cref="DeleteStockBalanceCommand"/> handler'ı. <see cref="IStockBalanceService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteStockBalanceCommandHandler
    : IRequestHandler<DeleteStockBalanceCommand, BaseResponse<bool>>
{
    private readonly IStockBalanceService _service;

    public DeleteStockBalanceCommandHandler(IStockBalanceService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteStockBalanceCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
