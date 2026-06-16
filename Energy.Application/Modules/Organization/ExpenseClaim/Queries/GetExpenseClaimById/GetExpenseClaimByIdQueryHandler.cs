using Energy.Application.Modules.Organization.ExpenseClaim.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.ExpenseClaim.Queries.GetExpenseClaimById;

/// <summary>
/// <see cref="GetExpenseClaimByIdQuery"/> handler'ı. <see cref="IExpenseClaimService"/>'i orkestre eder.
/// </summary>
public sealed class GetExpenseClaimByIdQueryHandler
    : IRequestHandler<GetExpenseClaimByIdQuery, BaseResponse<ExpenseClaimDetailResponse>>
{
    private readonly IExpenseClaimService _service;

    public GetExpenseClaimByIdQueryHandler(IExpenseClaimService service)
        => _service = service;

    public Task<BaseResponse<ExpenseClaimDetailResponse>> Handle(
        GetExpenseClaimByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
