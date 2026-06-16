using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.BudgetLine.Requests;
using Energy.Shared.Models.V1.Budget.BudgetLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Budget.BudgetLine.Queries.GetBudgetLineList;

/// <summary>Sayfalanmış BudgetLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetBudgetLineListQuery(GetBudgetLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<BudgetLineListResponse>>>;
