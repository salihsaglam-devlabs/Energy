using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocument.Requests;
using Energy.Shared.Models.V1.Inventory.StockDocument.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Inventory.StockDocument;

/// <summary>StockDocument API istemci sözleşmesi.</summary>
public interface IStockDocumentApiClient
{
    Task<BaseResponse<PaginatedResponse<StockDocumentListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<StockDocumentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<StockDocumentLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateStockDocumentRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockDocumentRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>StockDocument API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class StockDocumentApiClient : ApiClientBase, IStockDocumentApiClient
{
    private const string Base = "api/v1/inventory/stock-documents";

    public StockDocumentApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<StockDocumentListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<StockDocumentListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<StockDocumentDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<StockDocumentDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<StockDocumentLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<StockDocumentLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateStockDocumentRequest request, CancellationToken ct = default)
        => PostAsync<CreateStockDocumentRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockDocumentRequest request, CancellationToken ct = default)
        => PutAsync<UpdateStockDocumentRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
