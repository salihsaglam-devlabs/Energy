using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Requests;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Finance.FinancialTransactionLine;

/// <summary>FinancialTransactionLine API istemci sözleşmesi.</summary>
public interface IFinancialTransactionLineApiClient
{
    Task<BaseResponse<PaginatedResponse<FinancialTransactionLineListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<FinancialTransactionLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<FinancialTransactionLineLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateFinancialTransactionLineRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateFinancialTransactionLineRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>FinancialTransactionLine API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class FinancialTransactionLineApiClient : ApiClientBase, IFinancialTransactionLineApiClient
{
    private const string Base = "api/v1/finance/financial-transaction-lines";

    public FinancialTransactionLineApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<FinancialTransactionLineListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<FinancialTransactionLineListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<FinancialTransactionLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<FinancialTransactionLineDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<FinancialTransactionLineLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<FinancialTransactionLineLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateFinancialTransactionLineRequest request, CancellationToken ct = default)
        => PostAsync<CreateFinancialTransactionLineRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateFinancialTransactionLineRequest request, CancellationToken ct = default)
        => PutAsync<UpdateFinancialTransactionLineRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
