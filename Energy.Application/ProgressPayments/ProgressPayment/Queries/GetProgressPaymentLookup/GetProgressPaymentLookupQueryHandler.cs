using Energy.Application.ProgressPayments.ProgressPayment.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Responses;
using MediatR;

namespace Energy.Application.ProgressPayments.ProgressPayment.Queries.GetProgressPaymentLookup;

/// <summary>
/// <see cref="GetProgressPaymentLookupQuery"/> handler'ı. <see cref="IProgressPaymentLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetProgressPaymentLookupQueryHandler
    : IRequestHandler<GetProgressPaymentLookupQuery, BaseResponse<IReadOnlyList<ProgressPaymentLookupResponse>>>
{
    private readonly IProgressPaymentLookupService _lookup;

    public GetProgressPaymentLookupQueryHandler(IProgressPaymentLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ProgressPaymentLookupResponse>>> Handle(
        GetProgressPaymentLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
