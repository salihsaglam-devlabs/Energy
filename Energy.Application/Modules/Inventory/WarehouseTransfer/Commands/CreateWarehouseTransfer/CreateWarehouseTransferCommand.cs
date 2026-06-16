using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Requests;
using MediatR;

namespace Energy.Application.Modules.Inventory.WarehouseTransfer.Commands.CreateWarehouseTransfer;

/// <summary>Yeni WarehouseTransfer oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateWarehouseTransferCommand(CreateWarehouseTransferRequest Request)
    : IRequest<BaseResponse<Guid>>;
