using Energy.Application.Modules.Inventory.StockDocumentType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocumentType.Commands.UpdateStockDocumentType;

/// <summary>
/// <see cref="UpdateStockDocumentTypeCommand"/> handler'ı. <see cref="IStockDocumentTypeService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateStockDocumentTypeCommandHandler
    : IRequestHandler<UpdateStockDocumentTypeCommand, BaseResponse<bool>>
{
    private readonly IStockDocumentTypeService _service;

    public UpdateStockDocumentTypeCommandHandler(IStockDocumentTypeService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateStockDocumentTypeCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
