using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.Budget.Responses;
using MediatR;

namespace Energy.Application.Modules.Budget.Budget.Queries.GetBudgetById;

/// <summary>Kimliğe göre Budget detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetBudgetByIdQuery(Guid Id)
    : IRequest<BaseResponse<BudgetDetailResponse>>;
