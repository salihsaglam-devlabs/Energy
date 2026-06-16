using Energy.Application.Organization.ExpenseClaimLine.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Responses;
using MediatR;

namespace Energy.Application.Organization.ExpenseClaimLine.Queries.GetExpenseClaimLineLookup;

/// <summary>
/// <see cref="GetExpenseClaimLineLookupQuery"/> handler'ı. <see cref="IExpenseClaimLineLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetExpenseClaimLineLookupQueryHandler
    : IRequestHandler<GetExpenseClaimLineLookupQuery, BaseResponse<IReadOnlyList<ExpenseClaimLineLookupResponse>>>
{
    private readonly IExpenseClaimLineLookupService _lookup;

    public GetExpenseClaimLineLookupQueryHandler(IExpenseClaimLineLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ExpenseClaimLineLookupResponse>>> Handle(
        GetExpenseClaimLineLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
