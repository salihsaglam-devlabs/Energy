using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Requests;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Finance.FinancialAccount;

/// <summary>FinancialAccount API istemci sözleşmesi.</summary>
public interface IFinancialAccountApiClient
{
    Task<BaseResponse<PaginatedResponse<FinancialAccountListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<FinancialAccountDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<FinancialAccountLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateFinancialAccountRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateFinancialAccountRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>FinancialAccount API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class FinancialAccountApiClient : ApiClientBase, IFinancialAccountApiClient
{
    private const string Base = "api/v1/finance/financial-accounts";

    public FinancialAccountApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<FinancialAccountListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<FinancialAccountListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<FinancialAccountDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<FinancialAccountDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<FinancialAccountLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<FinancialAccountLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateFinancialAccountRequest request, CancellationToken ct = default)
        => PostAsync<CreateFinancialAccountRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateFinancialAccountRequest request, CancellationToken ct = default)
        => PutAsync<UpdateFinancialAccountRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
