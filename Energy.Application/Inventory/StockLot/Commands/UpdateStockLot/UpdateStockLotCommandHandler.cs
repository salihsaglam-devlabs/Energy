using Energy.Application.Inventory.StockLot.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockLot.Commands.UpdateStockLot;

/// <summary>
/// <see cref="UpdateStockLotCommand"/> handler'ı. <see cref="IStockLotService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateStockLotCommandHandler
    : IRequestHandler<UpdateStockLotCommand, BaseResponse<bool>>
{
    private readonly IStockLotService _service;

    public UpdateStockLotCommandHandler(IStockLotService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateStockLotCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
