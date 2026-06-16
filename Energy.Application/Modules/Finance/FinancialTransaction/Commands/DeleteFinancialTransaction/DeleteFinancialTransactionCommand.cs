using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.FinancialTransaction.Commands.DeleteFinancialTransaction;

/// <summary>FinancialTransaction kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteFinancialTransactionCommand(Guid Id) : IRequest<BaseResponse<bool>>;
