using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.FinancialTransactionLine.Commands.DeleteFinancialTransactionLine;

/// <summary>FinancialTransactionLine kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteFinancialTransactionLineCommand(Guid Id) : IRequest<BaseResponse<bool>>;
