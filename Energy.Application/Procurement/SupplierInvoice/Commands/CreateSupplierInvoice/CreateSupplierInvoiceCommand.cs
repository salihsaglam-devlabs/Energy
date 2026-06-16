using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Requests;
using MediatR;

namespace Energy.Application.Procurement.SupplierInvoice.Commands.CreateSupplierInvoice;

/// <summary>Yeni SupplierInvoice oluşturma use-case'i; yeni kimliği döndürür.</summary>
/// <param name="Request">Oluşturma alanlarını taşıyan istek modeli.</param>
public sealed record CreateSupplierInvoiceCommand(CreateSupplierInvoiceRequest Request)
    : IRequest<BaseResponse<Guid>>;
