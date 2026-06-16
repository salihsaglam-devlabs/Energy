using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierQuote.Queries.GetSupplierQuoteList;

/// <summary>Sayfalanmış SupplierQuote listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetSupplierQuoteListQuery(GetSupplierQuoteListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<SupplierQuoteListResponse>>>;
