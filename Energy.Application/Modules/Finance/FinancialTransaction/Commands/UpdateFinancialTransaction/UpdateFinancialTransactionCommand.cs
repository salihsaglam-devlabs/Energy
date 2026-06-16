using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Requests;
using MediatR;

namespace Energy.Application.Modules.Finance.FinancialTransaction.Commands.UpdateFinancialTransaction;

/// <summary>Var olan FinancialTransaction kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateFinancialTransactionCommand(Guid Id, UpdateFinancialTransactionRequest Request)
    : IRequest<BaseResponse<bool>>;
