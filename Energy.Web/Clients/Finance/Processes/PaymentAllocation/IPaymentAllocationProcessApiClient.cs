using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Requests;
using Energy.Shared.Models.V1.Finance.Processes.PaymentAllocation.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Finance.Processes.PaymentAllocation;

/// <summary>Ödeme tahsis süreci API istemci sözleşmesi.</summary>
public interface IPaymentAllocationProcessApiClient
{
    Task<BaseResponse<PaymentAllocationProcessResponse>> ExecuteAsync(PaymentAllocationProcessRequest request, CancellationToken ct = default);
}

/// <summary>Ödeme tahsis süreci API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class PaymentAllocationProcessApiClient : ApiClientBase, IPaymentAllocationProcessApiClient
{
    private const string Base = "api/v1/finance/processes/payment-allocation";

    public PaymentAllocationProcessApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaymentAllocationProcessResponse>> ExecuteAsync(PaymentAllocationProcessRequest request, CancellationToken ct = default)
        => PostAsync<PaymentAllocationProcessRequest, BaseResponse<PaymentAllocationProcessResponse>>(Base, request, ct);
}

