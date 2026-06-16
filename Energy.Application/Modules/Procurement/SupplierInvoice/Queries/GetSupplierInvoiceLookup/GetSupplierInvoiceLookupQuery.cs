using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierInvoice.Queries.GetSupplierInvoiceLookup;

/// <summary>SupplierInvoice lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetSupplierInvoiceLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<SupplierInvoiceLookupResponse>>>;
