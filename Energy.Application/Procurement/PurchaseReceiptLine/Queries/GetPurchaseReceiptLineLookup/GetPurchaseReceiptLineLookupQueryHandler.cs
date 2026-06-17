using Energy.Application.Procurement.PurchaseReceiptLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Responses;
using MediatR;

namespace Energy.Application.Procurement.PurchaseReceiptLine.Queries.GetPurchaseReceiptLineLookup;

/// <summary>
/// <see cref="GetPurchaseReceiptLineLookupQuery"/> handler'ı. <see cref="IPurchaseReceiptLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetPurchaseReceiptLineLookupQueryHandler
    : IRequestHandler<GetPurchaseReceiptLineLookupQuery, BaseResponse<IReadOnlyList<PurchaseReceiptLineLookupResponse>>>
{
    private readonly IPurchaseReceiptLineLookupService _lookup;

    public GetPurchaseReceiptLineLookupQueryHandler(IPurchaseReceiptLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<PurchaseReceiptLineLookupResponse>>> Handle(
        GetPurchaseReceiptLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
