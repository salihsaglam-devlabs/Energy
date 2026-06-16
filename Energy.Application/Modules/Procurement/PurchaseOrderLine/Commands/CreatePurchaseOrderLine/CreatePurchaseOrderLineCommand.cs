using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Requests;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrderLine.Commands.CreatePurchaseOrderLine;

/// <summary>Yeni PurchaseOrderLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreatePurchaseOrderLineCommand(CreatePurchaseOrderLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
