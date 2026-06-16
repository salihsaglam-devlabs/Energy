using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierInvoiceLine.Commands.DeleteSupplierInvoiceLine;

/// <summary>SupplierInvoiceLine kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteSupplierInvoiceLineCommand(Guid Id) : IRequest<BaseResponse<bool>>;
