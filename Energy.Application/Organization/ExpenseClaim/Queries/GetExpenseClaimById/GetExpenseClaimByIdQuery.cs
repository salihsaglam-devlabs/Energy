using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Responses;
using MediatR;

namespace Energy.Application.Organization.ExpenseClaim.Queries.GetExpenseClaimById;

/// <summary>Kimliğe göre ExpenseClaim detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetExpenseClaimByIdQuery(Guid Id)
    : IRequest<BaseResponse<ExpenseClaimDetailResponse>>;
