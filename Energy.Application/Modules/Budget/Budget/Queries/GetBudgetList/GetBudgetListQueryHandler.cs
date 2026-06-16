using Energy.Application.Modules.Budget.Budget.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.Budget.Responses;
using MediatR;

namespace Energy.Application.Modules.Budget.Budget.Queries.GetBudgetList;

/// <summary>
/// <see cref="GetBudgetListQuery"/> handler'ı. <see cref="IBudgetService"/>'i orkestre eder.
/// </summary>
public sealed class GetBudgetListQueryHandler
    : IRequestHandler<GetBudgetListQuery, BaseResponse<PaginatedResponse<BudgetListResponse>>>
{
    private readonly IBudgetService _service;

    public GetBudgetListQueryHandler(IBudgetService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<BudgetListResponse>>> Handle(
        GetBudgetListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
