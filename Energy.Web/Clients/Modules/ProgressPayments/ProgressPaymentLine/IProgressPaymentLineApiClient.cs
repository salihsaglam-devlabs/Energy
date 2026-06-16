using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Requests;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.ProgressPayments.ProgressPaymentLine;

/// <summary>ProgressPaymentLine API istemci sözleşmesi.</summary>
public interface IProgressPaymentLineApiClient
{
    Task<BaseResponse<PaginatedResponse<ProgressPaymentLineListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<ProgressPaymentLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ProgressPaymentLineLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateProgressPaymentLineRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProgressPaymentLineRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>ProgressPaymentLine API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ProgressPaymentLineApiClient : ApiClientBase, IProgressPaymentLineApiClient
{
    private const string Base = "api/v1/progress-payments/progress-payment-lines";

    public ProgressPaymentLineApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ProgressPaymentLineListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ProgressPaymentLineListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<ProgressPaymentLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<ProgressPaymentLineDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<ProgressPaymentLineLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ProgressPaymentLineLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateProgressPaymentLineRequest request, CancellationToken ct = default)
        => PostAsync<CreateProgressPaymentLineRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProgressPaymentLineRequest request, CancellationToken ct = default)
        => PutAsync<UpdateProgressPaymentLineRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
