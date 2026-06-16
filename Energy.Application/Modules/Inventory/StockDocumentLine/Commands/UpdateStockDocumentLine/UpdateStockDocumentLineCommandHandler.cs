using Energy.Application.Modules.Inventory.StockDocumentLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocumentLine.Commands.UpdateStockDocumentLine;

/// <summary>
/// <see cref="UpdateStockDocumentLineCommand"/> handler'ı. <see cref="IStockDocumentLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateStockDocumentLineCommandHandler
    : IRequestHandler<UpdateStockDocumentLineCommand, BaseResponse<bool>>
{
    private readonly IStockDocumentLineService _service;

    public UpdateStockDocumentLineCommandHandler(IStockDocumentLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateStockDocumentLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
