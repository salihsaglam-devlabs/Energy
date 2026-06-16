using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Requests;
using Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Responses;
using Energy.Application.Modules.Procurement.Reports.PurchaseOrderSummary.Services;
using MediatR;

namespace Energy.Application.Modules.Procurement.Reports.PurchaseOrderSummary.Queries.GetPurchaseOrderSummaryData;

/// <summary><see cref="GetPurchaseOrderSummaryDataQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetPurchaseOrderSummaryDataQueryHandler
    : IRequestHandler<GetPurchaseOrderSummaryDataQuery, BaseResponse<PaginatedResponse<PurchaseOrderSummaryRowResponse>>>
{
    private readonly IPurchaseOrderSummaryService _service;

    public GetPurchaseOrderSummaryDataQueryHandler(IPurchaseOrderSummaryService service)
    {
        _service = service;
    }

    public async Task<BaseResponse<PaginatedResponse<PurchaseOrderSummaryRowResponse>>> Handle(GetPurchaseOrderSummaryDataQuery request, CancellationToken ct)
    {
        return await _service.GetDataAsync(request.Request, ct);
    }
}
