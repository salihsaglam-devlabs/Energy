using Energy.Application.Modules.Procurement.SupplierInvoice.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Responses;
using MediatR;

namespace Energy.Application.Modules.Procurement.SupplierInvoice.Queries.GetSupplierInvoiceList;

/// <summary>
/// <see cref="GetSupplierInvoiceListQuery"/> handler'ı. <see cref="ISupplierInvoiceService"/>'i orkestre eder.
/// </summary>
public sealed class GetSupplierInvoiceListQueryHandler
    : IRequestHandler<GetSupplierInvoiceListQuery, BaseResponse<PaginatedResponse<SupplierInvoiceListResponse>>>
{
    private readonly ISupplierInvoiceService _service;

    public GetSupplierInvoiceListQueryHandler(ISupplierInvoiceService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<SupplierInvoiceListResponse>>> Handle(
        GetSupplierInvoiceListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
