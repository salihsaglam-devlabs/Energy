using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Organization.ExpenseClaimLine.Queries.GetExpenseClaimLineById;

/// <summary>Kimliğe göre ExpenseClaimLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetExpenseClaimLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<ExpenseClaimLineDetailResponse>>;
