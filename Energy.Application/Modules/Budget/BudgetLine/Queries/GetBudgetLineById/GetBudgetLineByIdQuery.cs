using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.BudgetLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Budget.BudgetLine.Queries.GetBudgetLineById;

/// <summary>Kimliğe göre BudgetLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetBudgetLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<BudgetLineDetailResponse>>;
