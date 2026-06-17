using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.ExpenseClaimLine.Commands.DeleteExpenseClaimLine;

/// <summary>ExpenseClaimLine kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteExpenseClaimLineCommand(Guid Id) : IRequest<BaseResponse<bool>>;
