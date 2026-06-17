using Energy.Application.Procurement.PurchaseReceipt.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.PurchaseReceipt.Commands.CreatePurchaseReceipt;

/// <summary>
/// <see cref="CreatePurchaseReceiptCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IPurchaseReceiptService"/>'i orkestre eder.
/// </summary>
public sealed class CreatePurchaseReceiptCommandHandler
    : IRequestHandler<CreatePurchaseReceiptCommand, BaseResponse<Guid>>
{
    private readonly IPurchaseReceiptService _service;

    public CreatePurchaseReceiptCommandHandler(IPurchaseReceiptService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreatePurchaseReceiptCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
