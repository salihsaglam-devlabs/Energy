using Energy.Application.Modules.Procurement.SupplierQuoteLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierQuoteLine.Queries.GetSupplierQuoteLineLookup;

/// <summary>
/// <see cref="GetSupplierQuoteLineLookupQuery"/> handler'ı. <see cref="ISupplierQuoteLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetSupplierQuoteLineLookupQueryHandler
    : IRequestHandler<GetSupplierQuoteLineLookupQuery, BaseResponse<IReadOnlyList<SupplierQuoteLineLookupResponse>>>
{
    private readonly ISupplierQuoteLineLookupService _lookup;

    public GetSupplierQuoteLineLookupQueryHandler(ISupplierQuoteLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<SupplierQuoteLineLookupResponse>>> Handle(
        GetSupplierQuoteLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
