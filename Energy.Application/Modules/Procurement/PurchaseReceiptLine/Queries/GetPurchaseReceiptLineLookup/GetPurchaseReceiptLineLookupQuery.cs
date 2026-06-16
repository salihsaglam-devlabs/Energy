using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseReceiptLine.Queries.GetPurchaseReceiptLineLookup;

/// <summary>PurchaseReceiptLine lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetPurchaseReceiptLineLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<PurchaseReceiptLineLookupResponse>>>;
