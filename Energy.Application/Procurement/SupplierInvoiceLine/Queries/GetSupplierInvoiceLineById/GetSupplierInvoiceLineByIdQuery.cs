using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierInvoiceLine.Queries.GetSupplierInvoiceLineById;

/// <summary>Kimliğe göre SupplierInvoiceLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetSupplierInvoiceLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<SupplierInvoiceLineDetailResponse>>;
