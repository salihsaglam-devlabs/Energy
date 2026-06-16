using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.Receivable.Requests;
using Energy.Shared.Models.V1.Finance.Receivable.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Finance.Receivable;

/// <summary>Receivable API istemci sözleşmesi.</summary>
public interface IReceivableApiClient
{
    Task<BaseResponse<PaginatedResponse<ReceivableListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<ReceivableDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ReceivableLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateReceivableRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateReceivableRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>Receivable API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class ReceivableApiClient : ApiClientBase, IReceivableApiClient
{
    private const string Base = "api/v1/finance/receivables";

    public ReceivableApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<ReceivableListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<ReceivableListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<ReceivableDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<ReceivableDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<ReceivableLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ReceivableLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateReceivableRequest request, CancellationToken ct = default)
        => PostAsync<CreateReceivableRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateReceivableRequest request, CancellationToken ct = default)
        => PutAsync<UpdateReceivableRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
