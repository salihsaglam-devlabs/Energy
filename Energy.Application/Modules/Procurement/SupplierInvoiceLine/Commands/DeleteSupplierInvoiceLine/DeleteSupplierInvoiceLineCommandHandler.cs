using Energy.Application.Modules.Procurement.SupplierInvoiceLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierInvoiceLine.Commands.DeleteSupplierInvoiceLine;

/// <summary>
/// <see cref="DeleteSupplierInvoiceLineCommand"/> handler'ı. <see cref="ISupplierInvoiceLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteSupplierInvoiceLineCommandHandler
    : IRequestHandler<DeleteSupplierInvoiceLineCommand, BaseResponse<bool>>
{
    private readonly ISupplierInvoiceLineService _service;

    public DeleteSupplierInvoiceLineCommandHandler(ISupplierInvoiceLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteSupplierInvoiceLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
