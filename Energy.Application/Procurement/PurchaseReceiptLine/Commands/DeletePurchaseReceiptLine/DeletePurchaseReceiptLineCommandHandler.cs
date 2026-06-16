using Energy.Application.Procurement.PurchaseReceiptLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.PurchaseReceiptLine.Commands.DeletePurchaseReceiptLine;

/// <summary>
/// <see cref="DeletePurchaseReceiptLineCommand"/> handler'ı. <see cref="IPurchaseReceiptLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeletePurchaseReceiptLineCommandHandler
    : IRequestHandler<DeletePurchaseReceiptLineCommand, BaseResponse<bool>>
{
    private readonly IPurchaseReceiptLineService _service;

    public DeletePurchaseReceiptLineCommandHandler(IPurchaseReceiptLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeletePurchaseReceiptLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
