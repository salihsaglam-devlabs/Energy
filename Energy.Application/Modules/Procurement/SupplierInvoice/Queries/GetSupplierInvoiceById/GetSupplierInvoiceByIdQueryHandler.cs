using Energy.Application.Modules.Procurement.SupplierInvoice.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierInvoice.Queries.GetSupplierInvoiceById;

/// <summary>
/// <see cref="GetSupplierInvoiceByIdQuery"/> handler'ı. <see cref="ISupplierInvoiceService"/>'i orkestre eder.
/// </summary>
public sealed class GetSupplierInvoiceByIdQueryHandler
    : IRequestHandler<GetSupplierInvoiceByIdQuery, BaseResponse<SupplierInvoiceDetailResponse>>
{
    private readonly ISupplierInvoiceService _service;

    public GetSupplierInvoiceByIdQueryHandler(ISupplierInvoiceService service)
        => _service = service;

    public Task<BaseResponse<SupplierInvoiceDetailResponse>> Handle(
        GetSupplierInvoiceByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
