using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Requests;
using Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Responses;
using Energy.Application.Inventory.Reports.StockBalanceReport.Services;
using MediatR;

namespace Energy.Application.Inventory.Reports.StockBalanceReport.Queries.GetStockBalanceReportData;

/// <summary><see cref="GetStockBalanceReportDataQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetStockBalanceReportDataQueryHandler
    : IRequestHandler<GetStockBalanceReportDataQuery, BaseResponse<PaginatedResponse<StockBalanceReportRowResponse>>>
{
    private readonly IStockBalanceReportService _service;

    public GetStockBalanceReportDataQueryHandler(IStockBalanceReportService service)
    {
        _service = service;
    }

    public async Task<BaseResponse<PaginatedResponse<StockBalanceReportRowResponse>>> Handle(GetStockBalanceReportDataQuery request, CancellationToken ct)
    {
        return await _service.GetDataAsync(request.Request, ct);
    }
}
