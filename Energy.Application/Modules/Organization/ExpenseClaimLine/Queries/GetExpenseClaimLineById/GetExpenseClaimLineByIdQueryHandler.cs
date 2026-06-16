using Energy.Application.Modules.Organization.ExpenseClaimLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.ExpenseClaimLine.Queries.GetExpenseClaimLineById;

/// <summary>
/// <see cref="GetExpenseClaimLineByIdQuery"/> handler'ı. <see cref="IExpenseClaimLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetExpenseClaimLineByIdQueryHandler
    : IRequestHandler<GetExpenseClaimLineByIdQuery, BaseResponse<ExpenseClaimLineDetailResponse>>
{
    private readonly IExpenseClaimLineService _service;

    public GetExpenseClaimLineByIdQueryHandler(IExpenseClaimLineService service)
        => _service = service;

    public Task<BaseResponse<ExpenseClaimLineDetailResponse>> Handle(
        GetExpenseClaimLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
