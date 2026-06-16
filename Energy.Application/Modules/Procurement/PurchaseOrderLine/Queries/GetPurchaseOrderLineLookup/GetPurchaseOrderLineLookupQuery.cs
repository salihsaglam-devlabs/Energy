using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.PurchaseOrderLine.Queries.GetPurchaseOrderLineLookup;

/// <summary>PurchaseOrderLine lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetPurchaseOrderLineLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<PurchaseOrderLineLookupResponse>>>;
