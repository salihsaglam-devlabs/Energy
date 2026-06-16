using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Budget.BudgetLine.Commands.DeleteBudgetLine;

/// <summary>BudgetLine kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteBudgetLineCommand(Guid Id) : IRequest<BaseResponse<bool>>;
