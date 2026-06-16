using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Requests;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Inventory.StockIssueAllocation;

/// <summary>StockIssueAllocation API istemci sözleşmesi.</summary>
public interface IStockIssueAllocationApiClient
{
    Task<BaseResponse<PaginatedResponse<StockIssueAllocationListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<StockIssueAllocationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<StockIssueAllocationLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateStockIssueAllocationRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockIssueAllocationRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>StockIssueAllocation API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class StockIssueAllocationApiClient : ApiClientBase, IStockIssueAllocationApiClient
{
    private const string Base = "api/v1/inventory/stock-issue-allocations";

    public StockIssueAllocationApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<StockIssueAllocationListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<StockIssueAllocationListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<StockIssueAllocationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<StockIssueAllocationDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<StockIssueAllocationLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<StockIssueAllocationLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateStockIssueAllocationRequest request, CancellationToken ct = default)
        => PostAsync<CreateStockIssueAllocationRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockIssueAllocationRequest request, CancellationToken ct = default)
        => PutAsync<UpdateStockIssueAllocationRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
