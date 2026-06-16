using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Requests;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.ProgressPayments.ProgressPaymentDeduction;

/// <summary>ProgressPaymentDeduction API istemci sözleşmesi.</summary>
public interface IProgressPaymentDeductionApiClient
{
    Task<BaseResponse<PaginatedResponse<ProgressPaymentDeductionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<ProgressPaymentDeductionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ProgressPaymentDeductionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateProgressPaymentDeductionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProgressPaymentDeductionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>ProgressPaymentDeduction API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ProgressPaymentDeductionApiClient : ApiClientBase, IProgressPaymentDeductionApiClient
{
    private const string Base = "api/v1/progress-payments/progress-payment-deductions";

    public ProgressPaymentDeductionApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ProgressPaymentDeductionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ProgressPaymentDeductionListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<ProgressPaymentDeductionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<ProgressPaymentDeductionDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<ProgressPaymentDeductionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ProgressPaymentDeductionLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateProgressPaymentDeductionRequest request, CancellationToken ct = default)
        => PostAsync<CreateProgressPaymentDeductionRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateProgressPaymentDeductionRequest request, CancellationToken ct = default)
        => PutAsync<UpdateProgressPaymentDeductionRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
