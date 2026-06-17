using Energy.Application.Finance.FinancialTransaction.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialTransaction.Queries.GetFinancialTransactionLookup;

/// <summary>
/// <see cref="GetFinancialTransactionLookupQuery"/> handler'ı. <see cref="IFinancialTransactionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetFinancialTransactionLookupQueryHandler
    : IRequestHandler<GetFinancialTransactionLookupQuery, BaseResponse<IReadOnlyList<FinancialTransactionLookupResponse>>>
{
    private readonly IFinancialTransactionLookupService _lookup;

    public GetFinancialTransactionLookupQueryHandler(IFinancialTransactionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<FinancialTransactionLookupResponse>>> Handle(
        GetFinancialTransactionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
