using Energy.Application.Finance.FinancialAccount.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialAccount.Queries.GetFinancialAccountLookup;

/// <summary>
/// <see cref="GetFinancialAccountLookupQuery"/> handler'ı. <see cref="IFinancialAccountLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetFinancialAccountLookupQueryHandler
    : IRequestHandler<GetFinancialAccountLookupQuery, BaseResponse<IReadOnlyList<FinancialAccountLookupResponse>>>
{
    private readonly IFinancialAccountLookupService _lookup;

    public GetFinancialAccountLookupQueryHandler(IFinancialAccountLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<FinancialAccountLookupResponse>>> Handle(
        GetFinancialAccountLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
