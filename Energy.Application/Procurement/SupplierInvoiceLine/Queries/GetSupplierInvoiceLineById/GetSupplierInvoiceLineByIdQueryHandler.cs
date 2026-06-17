using Energy.Application.Procurement.SupplierInvoiceLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierInvoiceLine.Queries.GetSupplierInvoiceLineById;

/// <summary>
/// <see cref="GetSupplierInvoiceLineByIdQuery"/> handler'ı. <see cref="ISupplierInvoiceLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetSupplierInvoiceLineByIdQueryHandler
    : IRequestHandler<GetSupplierInvoiceLineByIdQuery, BaseResponse<SupplierInvoiceLineDetailResponse>>
{
    private readonly ISupplierInvoiceLineService _service;

    public GetSupplierInvoiceLineByIdQueryHandler(ISupplierInvoiceLineService service)
        => _service = service;

    public Task<BaseResponse<SupplierInvoiceLineDetailResponse>> Handle(
        GetSupplierInvoiceLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
