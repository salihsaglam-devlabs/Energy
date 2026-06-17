using Energy.Application.Budget.BudgetLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.BudgetLine.Responses;
using MediatR;

namespace Energy.Application.Budget.BudgetLine.Queries.GetBudgetLineById;

/// <summary>
/// <see cref="GetBudgetLineByIdQuery"/> handler'ı. <see cref="IBudgetLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetBudgetLineByIdQueryHandler
    : IRequestHandler<GetBudgetLineByIdQuery, BaseResponse<BudgetLineDetailResponse>>
{
    private readonly IBudgetLineService _service;

    public GetBudgetLineByIdQueryHandler(IBudgetLineService service)
        => _service = service;

    public Task<BaseResponse<BudgetLineDetailResponse>> Handle(
        GetBudgetLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
