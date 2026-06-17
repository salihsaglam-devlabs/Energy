using Energy.Application.Procurement.SupplierInvoiceLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierInvoiceLine.Commands.UpdateSupplierInvoiceLine;

/// <summary>
/// <see cref="UpdateSupplierInvoiceLineCommand"/> handler'ı. <see cref="ISupplierInvoiceLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateSupplierInvoiceLineCommandHandler
    : IRequestHandler<UpdateSupplierInvoiceLineCommand, BaseResponse<bool>>
{
    private readonly ISupplierInvoiceLineService _service;

    public UpdateSupplierInvoiceLineCommandHandler(ISupplierInvoiceLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateSupplierInvoiceLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
