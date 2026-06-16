using Energy.Application.Modules.Organization.ExpenseClaimLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.ExpenseClaimLine.Queries.GetExpenseClaimLineList;

/// <summary>
/// <see cref="GetExpenseClaimLineListQuery"/> handler'ı. <see cref="IExpenseClaimLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetExpenseClaimLineListQueryHandler
    : IRequestHandler<GetExpenseClaimLineListQuery, BaseResponse<PaginatedResponse<ExpenseClaimLineListResponse>>>
{
    private readonly IExpenseClaimLineService _service;

    public GetExpenseClaimLineListQueryHandler(IExpenseClaimLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ExpenseClaimLineListResponse>>> Handle(
        GetExpenseClaimLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
