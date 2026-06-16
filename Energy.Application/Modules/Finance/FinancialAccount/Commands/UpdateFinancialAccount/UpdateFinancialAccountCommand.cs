using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Requests;
using MediatR;

namespace Energy.Application.Modules.Finance.FinancialAccount.Commands.UpdateFinancialAccount;

/// <summary>Var olan FinancialAccount kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateFinancialAccountCommand(Guid Id, UpdateFinancialAccountRequest Request)
    : IRequest<BaseResponse<bool>>;
