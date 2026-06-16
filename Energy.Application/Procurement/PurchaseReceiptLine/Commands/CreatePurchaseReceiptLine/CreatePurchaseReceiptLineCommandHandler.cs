using Energy.Application.Procurement.PurchaseReceiptLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.PurchaseReceiptLine.Commands.CreatePurchaseReceiptLine;

/// <summary>
/// <see cref="CreatePurchaseReceiptLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IPurchaseReceiptLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreatePurchaseReceiptLineCommandHandler
    : IRequestHandler<CreatePurchaseReceiptLineCommand, BaseResponse<Guid>>
{
    private readonly IPurchaseReceiptLineService _service;

    public CreatePurchaseReceiptLineCommandHandler(IPurchaseReceiptLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreatePurchaseReceiptLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
