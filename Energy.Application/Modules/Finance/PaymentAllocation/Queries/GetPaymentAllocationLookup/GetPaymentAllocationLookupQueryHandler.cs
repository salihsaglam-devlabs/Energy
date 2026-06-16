using Energy.Application.Modules.Finance.PaymentAllocation.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.PaymentAllocation.Queries.GetPaymentAllocationLookup;

/// <summary>
/// <see cref="GetPaymentAllocationLookupQuery"/> handler'ı. <see cref="IPaymentAllocationLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetPaymentAllocationLookupQueryHandler
    : IRequestHandler<GetPaymentAllocationLookupQuery, BaseResponse<IReadOnlyList<PaymentAllocationLookupResponse>>>
{
    private readonly IPaymentAllocationLookupService _lookup;

    public GetPaymentAllocationLookupQueryHandler(IPaymentAllocationLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<PaymentAllocationLookupResponse>>> Handle(
        GetPaymentAllocationLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
