using Energy.Application.Budget.Budget.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.Budget.Responses;
using MediatR;

namespace Energy.Application.Budget.Budget.Queries.GetBudgetLookup;

/// <summary>
/// <see cref="GetBudgetLookupQuery"/> handler'ı. <see cref="IBudgetLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetBudgetLookupQueryHandler
    : IRequestHandler<GetBudgetLookupQuery, BaseResponse<IReadOnlyList<BudgetLookupResponse>>>
{
    private readonly IBudgetLookupService _lookup;

    public GetBudgetLookupQueryHandler(IBudgetLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<BudgetLookupResponse>>> Handle(
        GetBudgetLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
