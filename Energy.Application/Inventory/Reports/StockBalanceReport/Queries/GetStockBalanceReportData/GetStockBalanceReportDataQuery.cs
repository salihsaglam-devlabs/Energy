using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Requests;
using Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Responses;
using MediatR;

namespace Energy.Application.Inventory.Reports.StockBalanceReport.Queries.GetStockBalanceReportData;

/// <summary>StockBalanceReport rapor verisi (filtreli, sayfalı).</summary>
public sealed record GetStockBalanceReportDataQuery(StockBalanceReportRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<StockBalanceReportRowResponse>>>;
