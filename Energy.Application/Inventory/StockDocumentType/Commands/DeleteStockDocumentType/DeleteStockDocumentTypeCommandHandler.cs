using Energy.Application.Inventory.StockDocumentType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocumentType.Commands.DeleteStockDocumentType;

/// <summary>
/// <see cref="DeleteStockDocumentTypeCommand"/> handler'ı. <see cref="IStockDocumentTypeService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteStockDocumentTypeCommandHandler
    : IRequestHandler<DeleteStockDocumentTypeCommand, BaseResponse<bool>>
{
    private readonly IStockDocumentTypeService _service;

    public DeleteStockDocumentTypeCommandHandler(IStockDocumentTypeService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteStockDocumentTypeCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
