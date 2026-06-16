using Energy.Application.ProgressPayments.ProgressPaymentDeduction.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Responses;
using MediatR;

namespace Energy.Application.ProgressPayments.ProgressPaymentDeduction.Queries.GetProgressPaymentDeductionLookup;

/// <summary>
/// <see cref="GetProgressPaymentDeductionLookupQuery"/> handler'ı. <see cref="IProgressPaymentDeductionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetProgressPaymentDeductionLookupQueryHandler
    : IRequestHandler<GetProgressPaymentDeductionLookupQuery, BaseResponse<IReadOnlyList<ProgressPaymentDeductionLookupResponse>>>
{
    private readonly IProgressPaymentDeductionLookupService _lookup;

    public GetProgressPaymentDeductionLookupQueryHandler(IProgressPaymentDeductionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ProgressPaymentDeductionLookupResponse>>> Handle(
        GetProgressPaymentDeductionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
