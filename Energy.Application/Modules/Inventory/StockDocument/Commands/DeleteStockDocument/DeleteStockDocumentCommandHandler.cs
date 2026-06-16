using Energy.Application.Modules.Inventory.StockDocument.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocument.Commands.DeleteStockDocument;

/// <summary>
/// <see cref="DeleteStockDocumentCommand"/> handler'ı. <see cref="IStockDocumentService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteStockDocumentCommandHandler
    : IRequestHandler<DeleteStockDocumentCommand, BaseResponse<bool>>
{
    private readonly IStockDocumentService _service;

    public DeleteStockDocumentCommandHandler(IStockDocumentService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteStockDocumentCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
