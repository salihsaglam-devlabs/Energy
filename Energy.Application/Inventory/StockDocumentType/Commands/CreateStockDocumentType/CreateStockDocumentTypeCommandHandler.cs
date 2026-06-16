using Energy.Application.Inventory.StockDocumentType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocumentType.Commands.CreateStockDocumentType;

/// <summary>
/// <see cref="CreateStockDocumentTypeCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IStockDocumentTypeService"/>'i orkestre eder.
/// </summary>
public sealed class CreateStockDocumentTypeCommandHandler
    : IRequestHandler<CreateStockDocumentTypeCommand, BaseResponse<Guid>>
{
    private readonly IStockDocumentTypeService _service;

    public CreateStockDocumentTypeCommandHandler(IStockDocumentTypeService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateStockDocumentTypeCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
