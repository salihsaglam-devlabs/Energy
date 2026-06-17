using Energy.Application.Procurement.SupplierQuoteLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;
using MediatR;

namespace Energy.Application.Procurement.SupplierQuoteLine.Queries.GetSupplierQuoteLineList;

/// <summary>
/// <see cref="GetSupplierQuoteLineListQuery"/> handler'ı. <see cref="ISupplierQuoteLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetSupplierQuoteLineListQueryHandler
    : IRequestHandler<GetSupplierQuoteLineListQuery, BaseResponse<PaginatedResponse<SupplierQuoteLineListResponse>>>
{
    private readonly ISupplierQuoteLineService _service;

    public GetSupplierQuoteLineListQueryHandler(ISupplierQuoteLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<SupplierQuoteLineListResponse>>> Handle(
        GetSupplierQuoteLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
