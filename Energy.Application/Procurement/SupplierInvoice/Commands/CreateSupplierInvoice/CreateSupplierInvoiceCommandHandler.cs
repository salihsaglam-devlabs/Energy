using Energy.Application.Procurement.SupplierInvoice.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierInvoice.Commands.CreateSupplierInvoice;

/// <summary>
/// <see cref="CreateSupplierInvoiceCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ISupplierInvoiceService"/>'i orkestre eder.
/// </summary>
public sealed class CreateSupplierInvoiceCommandHandler
    : IRequestHandler<CreateSupplierInvoiceCommand, BaseResponse<Guid>>
{
    private readonly ISupplierInvoiceService _service;

    public CreateSupplierInvoiceCommandHandler(ISupplierInvoiceService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateSupplierInvoiceCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
