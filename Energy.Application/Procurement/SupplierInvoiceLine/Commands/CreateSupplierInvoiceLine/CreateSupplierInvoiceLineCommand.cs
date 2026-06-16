using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Requests;
using MediatR;

namespace Energy.Application.Procurement.SupplierInvoiceLine.Commands.CreateSupplierInvoiceLine;

/// <summary>Yeni SupplierInvoiceLine oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateSupplierInvoiceLineCommand(CreateSupplierInvoiceLineRequest Request)
    : IRequest<BaseResponse<Guid>>;
