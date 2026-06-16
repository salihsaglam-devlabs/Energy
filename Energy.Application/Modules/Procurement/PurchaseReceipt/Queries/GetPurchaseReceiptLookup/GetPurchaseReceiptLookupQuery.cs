using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseReceipt.Queries.GetPurchaseReceiptLookup;

/// <summary>PurchaseReceipt lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetPurchaseReceiptLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<PurchaseReceiptLookupResponse>>>;
