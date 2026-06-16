using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Requests;
using MediatR;

namespace Energy.Application.Procurement.PurchaseReceiptLine.Commands.CreatePurchaseReceiptLine;

/// <summary>Yeni PurchaseReceiptLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreatePurchaseReceiptLineCommand(CreatePurchaseReceiptLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
