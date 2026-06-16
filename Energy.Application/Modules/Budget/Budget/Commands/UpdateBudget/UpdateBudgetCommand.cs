using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Budget.Budget.Requests;
using MediatR;

namespace Energy.Application.Modules.Budget.Budget.Commands.UpdateBudget;

/// <summary>Var olan Budget kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateBudgetCommand(Guid Id, UpdateBudgetRequest Request)
    : IRequest<BaseResponse<bool>>;
