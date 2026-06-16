using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Requests;
using Energy.Shared.Models.V1.Inventory.Processes.StockIssue.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Inventory.Processes.StockIssue;

/// <summary>Stok çıkış süreci API istemci sözleşmesi.</summary>
public interface IStockIssueProcessApiClient
{
    Task<BaseResponse<StockIssueProcessResponse>> ExecuteAsync(StockIssueProcessRequest request, CancellationToken ct = default);
}

/// <summary>Stok çıkış süreci API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class StockIssueProcessApiClient : ApiClientBase, IStockIssueProcessApiClient
{
    private const string Base = "api/v1/inventory/processes/stock-issue";

    public StockIssueProcessApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<StockIssueProcessResponse>> ExecuteAsync(StockIssueProcessRequest request, CancellationToken ct = default)
        => PostAsync<StockIssueProcessRequest, BaseResponse<StockIssueProcessResponse>>(Base, request, ct);
}

