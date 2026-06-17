using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.ExchangeRate.Requests;
using Energy.Shared.Models.V1.Core.ExchangeRate.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Core.ExchangeRate;

/// <summary>ExchangeRate API istemci sözleşmesi.</summary>
public interface IExchangeRateApiClient
{
    Task<BaseResponse<PaginatedResponse<ExchangeRateListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<ExchangeRateDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ExchangeRateLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateExchangeRateRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateExchangeRateRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>ExchangeRate API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ExchangeRateApiClient : ApiClientBase, IExchangeRateApiClient
{
    private const string Base = "api/v1/core/exchange-rates";

    public ExchangeRateApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ExchangeRateListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ExchangeRateListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<ExchangeRateDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<ExchangeRateDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<ExchangeRateLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ExchangeRateLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateExchangeRateRequest request, CancellationToken ct = default)
        => PostAsync<CreateExchangeRateRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateExchangeRateRequest request, CancellationToken ct = default)
        => PutAsync<UpdateExchangeRateRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
