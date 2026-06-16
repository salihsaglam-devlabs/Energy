using Energy.Application.Modules.Budget.BudgetLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.BudgetLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Budget.BudgetLine.Queries.GetBudgetLineLookup;

/// <summary>
/// <see cref="GetBudgetLineLookupQuery"/> handler'ı. <see cref="IBudgetLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetBudgetLineLookupQueryHandler
    : IRequestHandler<GetBudgetLineLookupQuery, BaseResponse<IReadOnlyList<BudgetLineLookupResponse>>>
{
    private readonly IBudgetLineLookupService _lookup;

    public GetBudgetLineLookupQueryHandler(IBudgetLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<BudgetLineLookupResponse>>> Handle(
        GetBudgetLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
