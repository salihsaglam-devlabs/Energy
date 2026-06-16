using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierQuote.Queries.GetSupplierQuoteLookup;

/// <summary>SupplierQuote lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetSupplierQuoteLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<SupplierQuoteLookupResponse>>>;
