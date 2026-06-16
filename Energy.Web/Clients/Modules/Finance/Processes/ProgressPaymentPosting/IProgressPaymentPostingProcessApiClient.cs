using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Requests;
using Energy.Shared.Models.V1.Finance.Processes.ProgressPaymentPosting.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.Finance.Processes.ProgressPaymentPosting;

/// <summary>Hakediş muhasebeleştirme süreci API istemci sözleşmesi.</summary>
public interface IProgressPaymentPostingProcessApiClient
{
    Task<BaseResponse<ProgressPaymentPostingProcessResponse>> ExecuteAsync(ProgressPaymentPostingProcessRequest request, CancellationToken ct = default);
}

/// <summary>Hakediş muhasebeleştirme süreci API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ProgressPaymentPostingProcessApiClient : ApiClientBase, IProgressPaymentPostingProcessApiClient
{
    private const string Base = "api/v1/finance/processes/progress-payment-posting";

    public ProgressPaymentPostingProcessApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<ProgressPaymentPostingProcessResponse>> ExecuteAsync(ProgressPaymentPostingProcessRequest request, CancellationToken ct = default)
        => PostAsync<ProgressPaymentPostingProcessRequest, BaseResponse<ProgressPaymentPostingProcessResponse>>(Base, request, ct);
}

