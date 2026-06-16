using Energy.Application.Procurement.SupplierInvoice.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierInvoice.Commands.DeleteSupplierInvoice;

/// <summary>
/// <see cref="DeleteSupplierInvoiceCommand"/> handler'ı. <see cref="ISupplierInvoiceService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteSupplierInvoiceCommandHandler
    : IRequestHandler<DeleteSupplierInvoiceCommand, BaseResponse<bool>>
{
    private readonly ISupplierInvoiceService _service;

    public DeleteSupplierInvoiceCommandHandler(ISupplierInvoiceService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteSupplierInvoiceCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
