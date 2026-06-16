using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Requests;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Responses;
using MediatR;

namespace Energy.Application.Organization.ExpenseClaim.Queries.GetExpenseClaimList;

/// <summary>Sayfalanmış ExpenseClaim listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetExpenseClaimListQuery(GetExpenseClaimListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ExpenseClaimListResponse>>>;
