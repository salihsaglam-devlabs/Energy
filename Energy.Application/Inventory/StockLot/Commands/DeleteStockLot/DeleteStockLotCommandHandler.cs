using Energy.Application.Inventory.StockLot.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockLot.Commands.DeleteStockLot;

/// <summary>
/// <see cref="DeleteStockLotCommand"/> handler'ı. <see cref="IStockLotService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteStockLotCommandHandler
    : IRequestHandler<DeleteStockLotCommand, BaseResponse<bool>>
{
    private readonly IStockLotService _service;

    public DeleteStockLotCommandHandler(IStockLotService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteStockLotCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
