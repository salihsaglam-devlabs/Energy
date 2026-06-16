using Energy.Application.Modules.Procurement.SupplierInvoice.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierInvoice.Queries.GetSupplierInvoiceLookup;

/// <summary>
/// <see cref="GetSupplierInvoiceLookupQuery"/> handler'ı. <see cref="ISupplierInvoiceLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetSupplierInvoiceLookupQueryHandler
    : IRequestHandler<GetSupplierInvoiceLookupQuery, BaseResponse<IReadOnlyList<SupplierInvoiceLookupResponse>>>
{
    private readonly ISupplierInvoiceLookupService _lookup;

    public GetSupplierInvoiceLookupQueryHandler(ISupplierInvoiceLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<SupplierInvoiceLookupResponse>>> Handle(
        GetSupplierInvoiceLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
