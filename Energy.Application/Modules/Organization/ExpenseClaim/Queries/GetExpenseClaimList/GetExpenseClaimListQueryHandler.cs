using Energy.Application.Modules.Organization.ExpenseClaim.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.ExpenseClaim.Queries.GetExpenseClaimList;

/// <summary>
/// <see cref="GetExpenseClaimListQuery"/> handler'ı. <see cref="IExpenseClaimService"/>'i orkestre eder.
/// </summary>
public sealed class GetExpenseClaimListQueryHandler
    : IRequestHandler<GetExpenseClaimListQuery, BaseResponse<PaginatedResponse<ExpenseClaimListResponse>>>
{
    private readonly IExpenseClaimService _service;

    public GetExpenseClaimListQueryHandler(IExpenseClaimService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ExpenseClaimListResponse>>> Handle(
        GetExpenseClaimListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
