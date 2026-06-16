using Energy.Application.Modules.Inventory.StockDocumentLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocumentLine.Commands.DeleteStockDocumentLine;

/// <summary>
/// <see cref="DeleteStockDocumentLineCommand"/> handler'ı. <see cref="IStockDocumentLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteStockDocumentLineCommandHandler
    : IRequestHandler<DeleteStockDocumentLineCommand, BaseResponse<bool>>
{
    private readonly IStockDocumentLineService _service;

    public DeleteStockDocumentLineCommandHandler(IStockDocumentLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteStockDocumentLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
