using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Requests;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransferLine.Commands.CreateWarehouseTransferLine;

/// <summary>Yeni WarehouseTransferLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateWarehouseTransferLineCommand(CreateWarehouseTransferLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
