using Energy.Application.Inventory.StockBalance.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockBalance.Commands.CreateStockBalance;

/// <summary>
/// <see cref="CreateStockBalanceCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IStockBalanceService"/>'i orkestre eder.
/// </summary>
public sealed class CreateStockBalanceCommandHandler
    : IRequestHandler<CreateStockBalanceCommand, BaseResponse<Guid>>
{
    private readonly IStockBalanceService _service;

    public CreateStockBalanceCommandHandler(IStockBalanceService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateStockBalanceCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
