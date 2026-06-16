using Energy.Application.Inventory.StockDocument.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocument.Commands.UpdateStockDocument;

/// <summary>
/// <see cref="UpdateStockDocumentCommand"/> handler'ı. <see cref="IStockDocumentService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateStockDocumentCommandHandler
    : IRequestHandler<UpdateStockDocumentCommand, BaseResponse<bool>>
{
    private readonly IStockDocumentService _service;

    public UpdateStockDocumentCommandHandler(IStockDocumentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateStockDocumentCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
