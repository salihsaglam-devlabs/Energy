using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCount.Requests;
using Energy.Shared.Models.V1.Inventory.StockCount.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Inventory.StockCount;

/// <summary>StockCount API istemci sözleşmesi.</summary>
public interface IStockCountApiClient
{
    Task<BaseResponse<PaginatedResponse<StockCountListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<StockCountDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<StockCountLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateStockCountRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockCountRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>StockCount API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class StockCountApiClient : ApiClientBase, IStockCountApiClient
{
    private const string Base = "api/v1/inventory/stock-counts";

    public StockCountApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<StockCountListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<StockCountListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<StockCountDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<StockCountDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<StockCountLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<StockCountLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateStockCountRequest request, CancellationToken ct = default)
        => PostAsync<CreateStockCountRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockCountRequest request, CancellationToken ct = default)
        => PutAsync<UpdateStockCountRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
