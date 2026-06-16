using Energy.Application.Modules.Procurement.PurchaseOrderLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrderLine.Queries.GetPurchaseOrderLineLookup;

/// <summary>
/// <see cref="GetPurchaseOrderLineLookupQuery"/> handler'ı. <see cref="IPurchaseOrderLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetPurchaseOrderLineLookupQueryHandler
    : IRequestHandler<GetPurchaseOrderLineLookupQuery, BaseResponse<IReadOnlyList<PurchaseOrderLineLookupResponse>>>
{
    private readonly IPurchaseOrderLineLookupService _lookup;

    public GetPurchaseOrderLineLookupQueryHandler(IPurchaseOrderLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<PurchaseOrderLineLookupResponse>>> Handle(
        GetPurchaseOrderLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
