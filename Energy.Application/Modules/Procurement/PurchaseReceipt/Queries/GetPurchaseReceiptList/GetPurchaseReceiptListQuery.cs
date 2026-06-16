using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseReceipt.Queries.GetPurchaseReceiptList;

/// <summary>Sayfalanmış PurchaseReceipt listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetPurchaseReceiptListQuery(GetPurchaseReceiptListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<PurchaseReceiptListResponse>>>;
