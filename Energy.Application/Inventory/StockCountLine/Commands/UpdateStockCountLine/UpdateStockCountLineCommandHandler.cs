using Energy.Application.Inventory.StockCountLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockCountLine.Commands.UpdateStockCountLine;

/// <summary>
/// <see cref="UpdateStockCountLineCommand"/> handler'ı. <see cref="IStockCountLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateStockCountLineCommandHandler
    : IRequestHandler<UpdateStockCountLineCommand, BaseResponse<bool>>
{
    private readonly IStockCountLineService _service;

    public UpdateStockCountLineCommandHandler(IStockCountLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateStockCountLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
