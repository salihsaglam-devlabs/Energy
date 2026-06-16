using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockIssueAllocation.Commands.DeleteStockIssueAllocation;

/// <summary>StockIssueAllocation kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteStockIssueAllocationCommand(Guid Id) : IRequest<BaseResponse<bool>>;
