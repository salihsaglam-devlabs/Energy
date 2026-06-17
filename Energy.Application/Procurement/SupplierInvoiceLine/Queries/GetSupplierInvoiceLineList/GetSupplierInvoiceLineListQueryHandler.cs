using Energy.Application.Procurement.SupplierInvoiceLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierInvoiceLine.Queries.GetSupplierInvoiceLineList;

/// <summary>
/// <see cref="GetSupplierInvoiceLineListQuery"/> handler'ı. <see cref="ISupplierInvoiceLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetSupplierInvoiceLineListQueryHandler
    : IRequestHandler<GetSupplierInvoiceLineListQuery, BaseResponse<PaginatedResponse<SupplierInvoiceLineListResponse>>>
{
    private readonly ISupplierInvoiceLineService _service;

    public GetSupplierInvoiceLineListQueryHandler(ISupplierInvoiceLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<SupplierInvoiceLineListResponse>>> Handle(
        GetSupplierInvoiceLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
