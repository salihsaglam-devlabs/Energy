using Energy.Application.Modules.Finance.FinancialTransactionLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.FinancialTransactionLine.Queries.GetFinancialTransactionLineLookup;

/// <summary>
/// <see cref="GetFinancialTransactionLineLookupQuery"/> handler'ı. <see cref="IFinancialTransactionLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetFinancialTransactionLineLookupQueryHandler
    : IRequestHandler<GetFinancialTransactionLineLookupQuery, BaseResponse<IReadOnlyList<FinancialTransactionLineLookupResponse>>>
{
    private readonly IFinancialTransactionLineLookupService _lookup;

    public GetFinancialTransactionLineLookupQueryHandler(IFinancialTransactionLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<FinancialTransactionLineLookupResponse>>> Handle(
        GetFinancialTransactionLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
