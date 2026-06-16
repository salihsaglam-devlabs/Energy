using Energy.Application.Modules.Procurement.PurchaseReceiptLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseReceiptLine.Queries.GetPurchaseReceiptLineById;

/// <summary>
/// <see cref="GetPurchaseReceiptLineByIdQuery"/> handler'ı. <see cref="IPurchaseReceiptLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetPurchaseReceiptLineByIdQueryHandler
    : IRequestHandler<GetPurchaseReceiptLineByIdQuery, BaseResponse<PurchaseReceiptLineDetailResponse>>
{
    private readonly IPurchaseReceiptLineService _service;

    public GetPurchaseReceiptLineByIdQueryHandler(IPurchaseReceiptLineService service)
        => _service = service;

    public Task<BaseResponse<PurchaseReceiptLineDetailResponse>> Handle(
        GetPurchaseReceiptLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
