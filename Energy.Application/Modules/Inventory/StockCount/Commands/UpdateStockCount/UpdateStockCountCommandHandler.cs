using Energy.Application.Modules.Inventory.StockCount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockCount.Commands.UpdateStockCount;

/// <summary>
/// <see cref="UpdateStockCountCommand"/> handler'ı. <see cref="IStockCountService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateStockCountCommandHandler
    : IRequestHandler<UpdateStockCountCommand, BaseResponse<bool>>
{
    private readonly IStockCountService _service;

    public UpdateStockCountCommandHandler(IStockCountService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateStockCountCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
