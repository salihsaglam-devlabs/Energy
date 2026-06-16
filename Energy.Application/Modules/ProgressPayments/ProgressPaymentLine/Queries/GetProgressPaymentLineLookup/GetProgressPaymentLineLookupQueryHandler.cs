using Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Responses;
using MediatR;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Queries.GetProgressPaymentLineLookup;

/// <summary>
/// <see cref="GetProgressPaymentLineLookupQuery"/> handler'ı. <see cref="IProgressPaymentLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetProgressPaymentLineLookupQueryHandler
    : IRequestHandler<GetProgressPaymentLineLookupQuery, BaseResponse<IReadOnlyList<ProgressPaymentLineLookupResponse>>>
{
    private readonly IProgressPaymentLineLookupService _lookup;

    public GetProgressPaymentLineLookupQueryHandler(IProgressPaymentLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ProgressPaymentLineLookupResponse>>> Handle(
        GetProgressPaymentLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
