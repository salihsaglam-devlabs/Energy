using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.Processes.GoodsReceipt.Requests;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.Procurement.Processes.GoodsReceipt;

/// <summary>Mal kabul süreci API istemci sözleşmesi.</summary>
public interface IGoodsReceiptProcessApiClient
{
    Task<BaseResponse<bool>> ExecuteAsync(GoodsReceiptProcessRequest request, CancellationToken ct = default);
}

/// <summary>Mal kabul süreci API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class GoodsReceiptProcessApiClient : ApiClientBase, IGoodsReceiptProcessApiClient
{
    private const string Base = "api/v1/procurement/processes/goods-receipt";

    public GoodsReceiptProcessApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<bool>> ExecuteAsync(GoodsReceiptProcessRequest request, CancellationToken ct = default)
        => PostAsync<GoodsReceiptProcessRequest, BaseResponse<bool>>(Base, request, ct);
}

