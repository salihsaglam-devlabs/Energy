using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseReceiptLine.Queries.GetPurchaseReceiptLineList;

/// <summary>Sayfalanmış PurchaseReceiptLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetPurchaseReceiptLineListQuery(GetPurchaseReceiptLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<PurchaseReceiptLineListResponse>>>;
