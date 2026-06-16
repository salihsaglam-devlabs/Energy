using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Requests;
using MediatR;

namespace Energy.Application.Inventory.StockCountLine.Commands.CreateStockCountLine;

/// <summary>Yeni StockCountLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateStockCountLineCommand(CreateStockCountLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
