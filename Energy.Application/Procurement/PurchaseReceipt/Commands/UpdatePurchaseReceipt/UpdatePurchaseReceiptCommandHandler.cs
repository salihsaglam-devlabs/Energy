using Energy.Application.Procurement.PurchaseReceipt.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.PurchaseReceipt.Commands.UpdatePurchaseReceipt;

/// <summary>
/// <see cref="UpdatePurchaseReceiptCommand"/> handler'ı. <see cref="IPurchaseReceiptService"/>'i orkestre eder.
/// </summary>
public sealed class UpdatePurchaseReceiptCommandHandler
    : IRequestHandler<UpdatePurchaseReceiptCommand, BaseResponse<bool>>
{
    private readonly IPurchaseReceiptService _service;

    public UpdatePurchaseReceiptCommandHandler(IPurchaseReceiptService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdatePurchaseReceiptCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
