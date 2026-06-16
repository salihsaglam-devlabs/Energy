using Energy.Application.Budget.BudgetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.BudgetLine.Responses;
using MediatR;

namespace Energy.Application.Budget.BudgetLine.Queries.GetBudgetLineList;

/// <summary>
/// <see cref="GetBudgetLineListQuery"/> handler'ı. <see cref="IBudgetLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetBudgetLineListQueryHandler
    : IRequestHandler<GetBudgetLineListQuery, BaseResponse<PaginatedResponse<BudgetLineListResponse>>>
{
    private readonly IBudgetLineService _service;

    public GetBudgetLineListQueryHandler(IBudgetLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<BudgetLineListResponse>>> Handle(
        GetBudgetLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
