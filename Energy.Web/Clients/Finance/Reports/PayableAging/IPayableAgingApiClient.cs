using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Reports.PayableAging.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Finance.Reports.PayableAging;

/// <summary>PayableAging raporu API istemci sözleşmesi.</summary>
public interface IPayableAgingApiClient
{
    Task<BaseResponse<PaginatedResponse<PayableAgingRowResponse>>> GetDataAsync(string query, CancellationToken ct = default);
}

/// <summary>PayableAging raporu API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class PayableAgingApiClient : ApiClientBase, IPayableAgingApiClient
{
    private const string Base = "api/v1/finance/reports/payable-aging";

    public PayableAgingApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<PayableAgingRowResponse>>> GetDataAsync(string query, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<PayableAgingRowResponse>>>(string.IsNullOrEmpty(query) ? Base : $"{Base}?{query}", ct);
}
