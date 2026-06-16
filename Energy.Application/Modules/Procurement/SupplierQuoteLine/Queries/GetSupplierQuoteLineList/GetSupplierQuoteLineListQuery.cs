using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierQuoteLine.Queries.GetSupplierQuoteLineList;

/// <summary>Sayfalanmış SupplierQuoteLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetSupplierQuoteLineListQuery(GetSupplierQuoteLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<SupplierQuoteLineListResponse>>>;
