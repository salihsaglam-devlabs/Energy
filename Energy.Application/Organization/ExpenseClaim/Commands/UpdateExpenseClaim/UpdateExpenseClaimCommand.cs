using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Requests;
using MediatR;

namespace Energy.Application.Organization.ExpenseClaim.Commands.UpdateExpenseClaim;

/// <summary>Var olan ExpenseClaim kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateExpenseClaimCommand(Guid Id, UpdateExpenseClaimRequest Request)
    : IRequest<BaseResponse<bool>>;
