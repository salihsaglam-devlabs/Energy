using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Reports.ReceivableAging.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Finance.Reports.ReceivableAging;

/// <summary>ReceivableAging raporu API istemci sözleşmesi.</summary>
public interface IReceivableAgingApiClient
{
    Task<BaseResponse<PaginatedResponse<ReceivableAgingRowResponse>>> GetDataAsync(string query, CancellationToken ct = default);
}

/// <summary>ReceivableAging raporu API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ReceivableAgingApiClient : ApiClientBase, IReceivableAgingApiClient
{
    private const string Base = "api/v1/finance/reports/receivable-aging";

    public ReceivableAgingApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ReceivableAgingRowResponse>>> GetDataAsync(string query, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ReceivableAgingRowResponse>>>(string.IsNullOrEmpty(query) ? Base : $"{Base}?{query}", ct);
}
