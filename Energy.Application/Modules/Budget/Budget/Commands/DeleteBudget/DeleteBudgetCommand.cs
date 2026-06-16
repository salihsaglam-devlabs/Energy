using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Budget.Budget.Commands.DeleteBudget;

/// <summary>Budget kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteBudgetCommand(Guid Id) : IRequest<BaseResponse<bool>>;
