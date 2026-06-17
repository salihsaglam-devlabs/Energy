using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Organization.ExpenseClaim.Commands.DeleteExpenseClaim;

/// <summary>ExpenseClaim kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteExpenseClaimCommand(Guid Id) : IRequest<BaseResponse<bool>>;
