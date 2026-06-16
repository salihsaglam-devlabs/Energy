using Energy.Application.Modules.Procurement.SupplierInvoice.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierInvoice.Commands.UpdateSupplierInvoice;

/// <summary>
/// <see cref="UpdateSupplierInvoiceCommand"/> handler'ı. <see cref="ISupplierInvoiceService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateSupplierInvoiceCommandHandler
    : IRequestHandler<UpdateSupplierInvoiceCommand, BaseResponse<bool>>
{
    private readonly ISupplierInvoiceService _service;

    public UpdateSupplierInvoiceCommandHandler(ISupplierInvoiceService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateSupplierInvoiceCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
