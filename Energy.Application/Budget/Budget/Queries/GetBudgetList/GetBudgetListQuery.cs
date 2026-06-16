using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.Budget.Requests;
using Energy.Shared.Models.V1.Budget.Budget.Responses;
using MediatR;

namespace Energy.Application.Budget.Budget.Queries.GetBudgetList;

/// <summary>Sayfalanmış Budget listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetBudgetListQuery(GetBudgetListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<BudgetListResponse>>>;
