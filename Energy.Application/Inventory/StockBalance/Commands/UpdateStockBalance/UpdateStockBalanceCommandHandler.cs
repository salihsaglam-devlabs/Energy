using Energy.Application.Inventory.StockBalance.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockBalance.Commands.UpdateStockBalance;

/// <summary>
/// <see cref="UpdateStockBalanceCommand"/> handler'ı. <see cref="IStockBalanceService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateStockBalanceCommandHandler
    : IRequestHandler<UpdateStockBalanceCommand, BaseResponse<bool>>
{
    private readonly IStockBalanceService _service;

    public UpdateStockBalanceCommandHandler(IStockBalanceService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateStockBalanceCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
