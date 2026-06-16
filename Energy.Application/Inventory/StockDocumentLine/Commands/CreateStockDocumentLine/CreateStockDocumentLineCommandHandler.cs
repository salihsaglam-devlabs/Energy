using Energy.Application.Inventory.StockDocumentLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocumentLine.Commands.CreateStockDocumentLine;

/// <summary>
/// <see cref="CreateStockDocumentLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IStockDocumentLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreateStockDocumentLineCommandHandler
    : IRequestHandler<CreateStockDocumentLineCommand, BaseResponse<Guid>>
{
    private readonly IStockDocumentLineService _service;

    public CreateStockDocumentLineCommandHandler(IStockDocumentLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateStockDocumentLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
