using Energy.Application.Modules.Procurement.SupplierInvoiceLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierInvoiceLine.Queries.GetSupplierInvoiceLineLookup;

/// <summary>
/// <see cref="GetSupplierInvoiceLineLookupQuery"/> handler'ı. <see cref="ISupplierInvoiceLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetSupplierInvoiceLineLookupQueryHandler
    : IRequestHandler<GetSupplierInvoiceLineLookupQuery, BaseResponse<IReadOnlyList<SupplierInvoiceLineLookupResponse>>>
{
    private readonly ISupplierInvoiceLineLookupService _lookup;

    public GetSupplierInvoiceLineLookupQueryHandler(ISupplierInvoiceLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<SupplierInvoiceLineLookupResponse>>> Handle(
        GetSupplierInvoiceLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
