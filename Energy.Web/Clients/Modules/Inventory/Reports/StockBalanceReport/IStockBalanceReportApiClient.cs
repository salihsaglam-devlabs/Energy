using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Reports.StockBalanceReport.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.Inventory.Reports.StockBalanceReport;

/// <summary>StockBalanceReport raporu API istemci sözleşmesi.</summary>
public interface IStockBalanceReportApiClient
{
    Task<BaseResponse<PaginatedResponse<StockBalanceReportRowResponse>>> GetDataAsync(string query, CancellationToken ct = default);
}

/// <summary>StockBalanceReport raporu API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class StockBalanceReportApiClient : ApiClientBase, IStockBalanceReportApiClient
{
    private const string Base = "api/v1/inventory/reports/stock-balance-report";

    public StockBalanceReportApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<StockBalanceReportRowResponse>>> GetDataAsync(string query, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<StockBalanceReportRowResponse>>>(string.IsNullOrEmpty(query) ? Base : $"{Base}?{query}", ct);
}
