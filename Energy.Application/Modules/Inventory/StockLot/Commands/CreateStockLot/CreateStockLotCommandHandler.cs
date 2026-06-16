using Energy.Application.Modules.Inventory.StockLot.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockLot.Commands.CreateStockLot;

/// <summary>
/// <see cref="CreateStockLotCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IStockLotService"/>'i orkestre eder.
/// </summary>
public sealed class CreateStockLotCommandHandler
    : IRequestHandler<CreateStockLotCommand, BaseResponse<Guid>>
{
    private readonly IStockLotService _service;

    public CreateStockLotCommandHandler(IStockLotService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateStockLotCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
