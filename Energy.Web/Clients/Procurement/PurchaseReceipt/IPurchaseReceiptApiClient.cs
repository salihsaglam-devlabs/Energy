using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Procurement.PurchaseReceipt;

/// <summary>PurchaseReceipt API istemci sözleşmesi.</summary>
public interface IPurchaseReceiptApiClient
{
    Task<BaseResponse<PaginatedResponse<PurchaseReceiptListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<PurchaseReceiptDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<PurchaseReceiptLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreatePurchaseReceiptRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePurchaseReceiptRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>PurchaseReceipt API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class PurchaseReceiptApiClient : ApiClientBase, IPurchaseReceiptApiClient
{
    private const string Base = "api/v1/procurement/purchase-receipts";

    public PurchaseReceiptApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<PurchaseReceiptListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<PurchaseReceiptListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<PurchaseReceiptDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<PurchaseReceiptDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<PurchaseReceiptLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<PurchaseReceiptLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreatePurchaseReceiptRequest request, CancellationToken ct = default)
        => PostAsync<CreatePurchaseReceiptRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePurchaseReceiptRequest request, CancellationToken ct = default)
        => PutAsync<UpdatePurchaseReceiptRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
