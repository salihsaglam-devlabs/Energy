using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Requests;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Finance.FinancialTransaction;

/// <summary>FinancialTransaction API istemci sözleşmesi.</summary>
public interface IFinancialTransactionApiClient
{
    Task<BaseResponse<PaginatedResponse<FinancialTransactionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<FinancialTransactionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<FinancialTransactionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateFinancialTransactionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateFinancialTransactionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>FinancialTransaction API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class FinancialTransactionApiClient : ApiClientBase, IFinancialTransactionApiClient
{
    private const string Base = "api/v1/finance/financial-transactions";

    public FinancialTransactionApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<FinancialTransactionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<FinancialTransactionListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<FinancialTransactionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<FinancialTransactionDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<FinancialTransactionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<FinancialTransactionLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateFinancialTransactionRequest request, CancellationToken ct = default)
        => PostAsync<CreateFinancialTransactionRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateFinancialTransactionRequest request, CancellationToken ct = default)
        => PutAsync<UpdateFinancialTransactionRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
