using Energy.Application.Inventory.StockDocument.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocument.Commands.CreateStockDocument;

/// <summary>
/// <see cref="CreateStockDocumentCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IStockDocumentService"/>'i orkestre eder.
/// </summary>
public sealed class CreateStockDocumentCommandHandler
    : IRequestHandler<CreateStockDocumentCommand, BaseResponse<Guid>>
{
    private readonly IStockDocumentService _service;

    public CreateStockDocumentCommandHandler(IStockDocumentService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateStockDocumentCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
