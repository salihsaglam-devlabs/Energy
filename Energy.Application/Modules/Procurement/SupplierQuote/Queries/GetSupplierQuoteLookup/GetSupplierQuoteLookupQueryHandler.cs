using Energy.Application.Modules.Procurement.SupplierQuote.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierQuote.Queries.GetSupplierQuoteLookup;

/// <summary>
/// <see cref="GetSupplierQuoteLookupQuery"/> handler'ı. <see cref="ISupplierQuoteLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetSupplierQuoteLookupQueryHandler
    : IRequestHandler<GetSupplierQuoteLookupQuery, BaseResponse<IReadOnlyList<SupplierQuoteLookupResponse>>>
{
    private readonly ISupplierQuoteLookupService _lookup;

    public GetSupplierQuoteLookupQueryHandler(ISupplierQuoteLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<SupplierQuoteLookupResponse>>> Handle(
        GetSupplierQuoteLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
