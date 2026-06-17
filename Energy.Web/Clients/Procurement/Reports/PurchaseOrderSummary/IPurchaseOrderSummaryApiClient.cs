using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.Reports.PurchaseOrderSummary.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Procurement.Reports.PurchaseOrderSummary;

/// <summary>PurchaseOrderSummary raporu API istemci sözleşmesi.</summary>
public interface IPurchaseOrderSummaryApiClient
{
    Task<BaseResponse<PaginatedResponse<PurchaseOrderSummaryRowResponse>>> GetDataAsync(string query, CancellationToken ct = default);
}

/// <summary>PurchaseOrderSummary raporu API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class PurchaseOrderSummaryApiClient : ApiClientBase, IPurchaseOrderSummaryApiClient
{
    private const string Base = "api/v1/procurement/reports/purchase-order-summary";

    public PurchaseOrderSummaryApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<PurchaseOrderSummaryRowResponse>>> GetDataAsync(string query, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<PurchaseOrderSummaryRowResponse>>>(string.IsNullOrEmpty(query) ? Base : $"{Base}?{query}", ct);
}
