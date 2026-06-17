using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierQuoteLine.Queries.GetSupplierQuoteLineLookup;

/// <summary>SupplierQuoteLine lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetSupplierQuoteLineLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<SupplierQuoteLineLookupResponse>>>;
