using Energy.Application.Modules.Inventory.StockCountLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockCountLine.Commands.DeleteStockCountLine;

/// <summary>
/// <see cref="DeleteStockCountLineCommand"/> handler'ı. <see cref="IStockCountLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteStockCountLineCommandHandler
    : IRequestHandler<DeleteStockCountLineCommand, BaseResponse<bool>>
{
    private readonly IStockCountLineService _service;

    public DeleteStockCountLineCommandHandler(IStockCountLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteStockCountLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
