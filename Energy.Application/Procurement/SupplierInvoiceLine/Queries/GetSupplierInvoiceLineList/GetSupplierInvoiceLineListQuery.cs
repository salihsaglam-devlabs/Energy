using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierInvoiceLine.Queries.GetSupplierInvoiceLineList;

/// <summary>Sayfalanmış SupplierInvoiceLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetSupplierInvoiceLineListQuery(GetSupplierInvoiceLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<SupplierInvoiceLineListResponse>>>;
