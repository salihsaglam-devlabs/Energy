using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.Reports.ProgressPaymentSummary.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.ProgressPayments.Reports.ProgressPaymentSummary;

/// <summary>ProgressPaymentSummary raporu API istemci sözleşmesi.</summary>
public interface IProgressPaymentSummaryApiClient
{
    Task<BaseResponse<PaginatedResponse<ProgressPaymentSummaryRowResponse>>> GetDataAsync(string query, CancellationToken ct = default);
}

/// <summary>ProgressPaymentSummary raporu API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ProgressPaymentSummaryApiClient : ApiClientBase, IProgressPaymentSummaryApiClient
{
    private const string Base = "api/v1/progress-payments/reports/progress-payment-summary";

    public ProgressPaymentSummaryApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ProgressPaymentSummaryRowResponse>>> GetDataAsync(string query, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ProgressPaymentSummaryRowResponse>>>(string.IsNullOrEmpty(query) ? Base : $"{Base}?{query}", ct);
}
