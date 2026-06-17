using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Requests;
using MediatR;

namespace Energy.Application.Procurement.SupplierInvoiceLine.Commands.UpdateSupplierInvoiceLine;

/// <summary>Var olan SupplierInvoiceLine kaydını güncelleme use-case'i.</summary>
/// <param name="Id">Güncellenecek kaydın kimliği.</param>
/// <param name="Request">Güncellenecek alanları taşıyan istek modeli.</param>
public sealed record UpdateSupplierInvoiceLineCommand(Guid Id, UpdateSupplierInvoiceLineRequest Request)
    : IRequest<BaseResponse<bool>>;
