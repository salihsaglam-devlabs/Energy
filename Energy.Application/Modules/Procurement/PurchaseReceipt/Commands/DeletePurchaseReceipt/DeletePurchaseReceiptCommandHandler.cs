using Energy.Application.Modules.Procurement.PurchaseReceipt.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseReceipt.Commands.DeletePurchaseReceipt;

/// <summary>
/// <see cref="DeletePurchaseReceiptCommand"/> handler'ı. <see cref="IPurchaseReceiptService"/>'i orkestre eder.
/// </summary>
public sealed class DeletePurchaseReceiptCommandHandler
    : IRequestHandler<DeletePurchaseReceiptCommand, BaseResponse<bool>>
{
    private readonly IPurchaseReceiptService _service;

    public DeletePurchaseReceiptCommandHandler(IPurchaseReceiptService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeletePurchaseReceiptCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
