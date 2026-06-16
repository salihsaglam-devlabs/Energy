using Energy.Application.Modules.Finance.Payment.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payment.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.Payment.Queries.GetPaymentLookup;

/// <summary>
/// <see cref="GetPaymentLookupQuery"/> handler'ı. <see cref="IPaymentLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetPaymentLookupQueryHandler
    : IRequestHandler<GetPaymentLookupQuery, BaseResponse<IReadOnlyList<PaymentLookupResponse>>>
{
    private readonly IPaymentLookupService _lookup;

    public GetPaymentLookupQueryHandler(IPaymentLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<PaymentLookupResponse>>> Handle(
        GetPaymentLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
