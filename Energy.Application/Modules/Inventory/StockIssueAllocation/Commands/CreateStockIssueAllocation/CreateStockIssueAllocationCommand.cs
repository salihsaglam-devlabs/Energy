using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Requests;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockIssueAllocation.Commands.CreateStockIssueAllocation;

/// <summary>Yeni StockIssueAllocation oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateStockIssueAllocationCommand(CreateStockIssueAllocationRequest Request)
    : IRequest<BaseResponse<Guid>>;
