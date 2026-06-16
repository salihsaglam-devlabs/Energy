using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Requests;
using MediatR;

namespace Energy.Application.Procurement.PurchaseOrder.Commands.CreatePurchaseOrder;

/// <summary>Yeni PurchaseOrder oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreatePurchaseOrderCommand(CreatePurchaseOrderRequest Request)
    : IRequest<BaseResponse<Guid>>;
