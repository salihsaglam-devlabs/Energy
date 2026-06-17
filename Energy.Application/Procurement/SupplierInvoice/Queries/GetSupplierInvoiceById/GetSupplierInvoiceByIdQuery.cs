using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierInvoice.Queries.GetSupplierInvoiceById;

/// <summary>Kimliğe göre SupplierInvoice detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetSupplierInvoiceByIdQuery(Guid Id)
    : IRequest<BaseResponse<SupplierInvoiceDetailResponse>>;
