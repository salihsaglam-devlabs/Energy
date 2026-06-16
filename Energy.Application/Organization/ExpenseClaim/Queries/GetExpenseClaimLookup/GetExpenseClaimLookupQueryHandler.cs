using Energy.Application.Organization.ExpenseClaim.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Responses;
using MediatR;

namespace Energy.Application.Organization.ExpenseClaim.Queries.GetExpenseClaimLookup;

/// <summary>
/// <see cref="GetExpenseClaimLookupQuery"/> handler'ı. <see cref="IExpenseClaimLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetExpenseClaimLookupQueryHandler
    : IRequestHandler<GetExpenseClaimLookupQuery, BaseResponse<IReadOnlyList<ExpenseClaimLookupResponse>>>
{
    private readonly IExpenseClaimLookupService _lookup;

    public GetExpenseClaimLookupQueryHandler(IExpenseClaimLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ExpenseClaimLookupResponse>>> Handle(
        GetExpenseClaimLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
