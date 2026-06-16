using Energy.Application.Modules.Procurement.SupplierInvoiceLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierInvoiceLine.Commands.CreateSupplierInvoiceLine;

/// <summary>
/// <see cref="CreateSupplierInvoiceLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ISupplierInvoiceLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreateSupplierInvoiceLineCommandHandler
    : IRequestHandler<CreateSupplierInvoiceLineCommand, BaseResponse<Guid>>
{
    private readonly ISupplierInvoiceLineService _service;

    public CreateSupplierInvoiceLineCommandHandler(ISupplierInvoiceLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateSupplierInvoiceLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
