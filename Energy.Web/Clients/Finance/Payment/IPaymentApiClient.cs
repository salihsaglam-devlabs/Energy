using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Payment.Requests;
using Energy.Shared.Models.V1.Finance.Payment.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Finance.Payment;

/// <summary>Payment API istemci sözleşmesi.</summary>
public interface IPaymentApiClient
{
    Task<BaseResponse<PaginatedResponse<PaymentListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<PaymentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<PaymentLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreatePaymentRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePaymentRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>Payment API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class PaymentApiClient : ApiClientBase, IPaymentApiClient
{
    private const string Base = "api/v1/finance/payments";

    public PaymentApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<PaymentListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<PaymentListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<PaymentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaymentDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<PaymentLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<PaymentLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreatePaymentRequest request, CancellationToken ct = default)
        => PostAsync<CreatePaymentRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePaymentRequest request, CancellationToken ct = default)
        => PutAsync<UpdatePaymentRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
