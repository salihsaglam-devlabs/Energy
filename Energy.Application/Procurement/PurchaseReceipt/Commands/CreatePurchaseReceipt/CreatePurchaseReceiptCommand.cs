using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Requests;
using MediatR;

namespace Energy.Application.Procurement.PurchaseReceipt.Commands.CreatePurchaseReceipt;

/// <summary>Yeni PurchaseReceipt oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreatePurchaseReceiptCommand(CreatePurchaseReceiptRequest Request)
    : IRequest<BaseResponse<Guid>>;
