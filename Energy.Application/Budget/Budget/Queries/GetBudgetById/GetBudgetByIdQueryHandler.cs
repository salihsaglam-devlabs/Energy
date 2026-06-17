using Energy.Application.Budget.Budget.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.Budget.Responses;
using MediatR;

namespace Energy.Application.Budget.Budget.Queries.GetBudgetById;

/// <summary>
/// <see cref="GetBudgetByIdQuery"/> handler'ı. <see cref="IBudgetService"/>'i orkestre eder.
/// </summary>
public sealed class GetBudgetByIdQueryHandler
    : IRequestHandler<GetBudgetByIdQuery, BaseResponse<BudgetDetailResponse>>
{
    private readonly IBudgetService _service;

    public GetBudgetByIdQueryHandler(IBudgetService service)
        => _service = service;

    public Task<BaseResponse<BudgetDetailResponse>> Handle(
        GetBudgetByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
