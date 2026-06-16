using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Requests;
using MediatR;

namespace Energy.Application.Modules.Finance.FinancialTransactionLine.Commands.UpdateFinancialTransactionLine;

/// <summary>Var olan FinancialTransactionLine kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateFinancialTransactionLineCommand(Guid Id, UpdateFinancialTransactionLineRequest Request)
    : IRequest<BaseResponse<bool>>;
