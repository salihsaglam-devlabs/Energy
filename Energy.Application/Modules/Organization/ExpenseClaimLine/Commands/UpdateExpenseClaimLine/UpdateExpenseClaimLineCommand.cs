using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Requests;
using MediatR;

namespace Energy.Application.Modules.Organization.ExpenseClaimLine.Commands.UpdateExpenseClaimLine;

/// <summary>Var olan ExpenseClaimLine kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateExpenseClaimLineCommand(Guid Id, UpdateExpenseClaimLineRequest Request)
    : IRequest<BaseResponse<bool>>;
