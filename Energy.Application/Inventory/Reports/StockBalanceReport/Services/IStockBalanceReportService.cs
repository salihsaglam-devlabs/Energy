using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Requests;
using Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Responses;

namespace Energy.Application.Inventory.Reports.StockBalanceReport.Services;

/// <summary>StockBalanceReport raporu servis sözleşmesi (salt-okunur).</summary>
public interface IStockBalanceReportService
{
    /// <summary>Filtrelenmiş, sayfalanmış rapor verisini döndürür.</summary>
    Task<BaseResponse<PaginatedResponse<StockBalanceReportRowResponse>>> GetDataAsync(StockBalanceReportRequest request, CancellationToken ct = default);
}
