using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Requests;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.ExpenseClaimLine.Queries.GetExpenseClaimLineList;

/// <summary>Sayfalanmış ExpenseClaimLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetExpenseClaimLineListQuery(GetExpenseClaimLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ExpenseClaimLineListResponse>>>;
