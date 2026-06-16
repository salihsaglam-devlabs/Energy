using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.BudgetLine.Requests;
using MediatR;

namespace Energy.Application.Budget.BudgetLine.Commands.UpdateBudgetLine;

/// <summary>Var olan BudgetLine kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateBudgetLineCommand(Guid Id, UpdateBudgetLineRequest Request)
    : IRequest<BaseResponse<bool>>;
