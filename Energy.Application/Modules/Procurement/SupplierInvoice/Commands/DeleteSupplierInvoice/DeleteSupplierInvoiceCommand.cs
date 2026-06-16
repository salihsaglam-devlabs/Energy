using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierInvoice.Commands.DeleteSupplierInvoice;

/// <summary>SupplierInvoice kaydını (gerekiyorsa soft-delete) silme use-case'i.</summary>
/// <param name="Id">Silinecek kaydın kimliği.</param>
public sealed record DeleteSupplierInvoiceCommand(Guid Id) : IRequest<BaseResponse<bool>>;
