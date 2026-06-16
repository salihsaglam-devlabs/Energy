using Energy.Application.Modules.Procurement.PurchaseReceiptLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseReceiptLine.Commands.UpdatePurchaseReceiptLine;

/// <summary>
/// <see cref="UpdatePurchaseReceiptLineCommand"/> handler'ı. <see cref="IPurchaseReceiptLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdatePurchaseReceiptLineCommandHandler
    : IRequestHandler<UpdatePurchaseReceiptLineCommand, BaseResponse<bool>>
{
    private readonly IPurchaseReceiptLineService _service;

    public UpdatePurchaseReceiptLineCommandHandler(IPurchaseReceiptLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdatePurchaseReceiptLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
