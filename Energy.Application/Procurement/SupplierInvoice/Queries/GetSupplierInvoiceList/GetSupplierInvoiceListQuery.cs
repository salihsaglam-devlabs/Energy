using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierInvoice.Queries.GetSupplierInvoiceList;

/// <summary>Sayfalanmış SupplierInvoice listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetSupplierInvoiceListQuery(GetSupplierInvoiceListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<SupplierInvoiceListResponse>>>;
