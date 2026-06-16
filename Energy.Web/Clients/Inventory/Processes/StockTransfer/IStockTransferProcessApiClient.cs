using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Requests;
using Energy.Shared.Models.V1.Inventory.Processes.StockTransfer.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Inventory.Processes.StockTransfer;

/// <summary>Stok transfer süreci API istemci sözleşmesi.</summary>
public interface IStockTransferProcessApiClient
{
    Task<BaseResponse<StockTransferProcessResponse>> ExecuteAsync(StockTransferProcessRequest request, CancellationToken ct = default);
}

/// <summary>Stok transfer süreci API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class StockTransferProcessApiClient : ApiClientBase, IStockTransferProcessApiClient
{
    private const string Base = "api/v1/inventory/processes/stock-transfer";

    public StockTransferProcessApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<StockTransferProcessResponse>> ExecuteAsync(StockTransferProcessRequest request, CancellationToken ct = default)
        => PostAsync<StockTransferProcessRequest, BaseResponse<StockTransferProcessResponse>>(Base, request, ct);
}

