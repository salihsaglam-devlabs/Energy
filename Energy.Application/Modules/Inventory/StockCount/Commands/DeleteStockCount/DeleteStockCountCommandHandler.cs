using Energy.Application.Modules.Inventory.StockCount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockCount.Commands.DeleteStockCount;

/// <summary>
/// <see cref="DeleteStockCountCommand"/> handler'ı. <see cref="IStockCountService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteStockCountCommandHandler
    : IRequestHandler<DeleteStockCountCommand, BaseResponse<bool>>
{
    private readonly IStockCountService _service;

    public DeleteStockCountCommandHandler(IStockCountService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteStockCountCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
