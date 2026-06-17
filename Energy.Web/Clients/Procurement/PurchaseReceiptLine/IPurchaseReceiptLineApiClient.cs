using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Procurement.PurchaseReceiptLine;

/// <summary>PurchaseReceiptLine API istemci sözleşmesi.</summary>
public interface IPurchaseReceiptLineApiClient
{
    Task<BaseResponse<PaginatedResponse<PurchaseReceiptLineListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<PurchaseReceiptLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<PurchaseReceiptLineLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreatePurchaseReceiptLineRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePurchaseReceiptLineRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>PurchaseReceiptLine API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class PurchaseReceiptLineApiClient : ApiClientBase, IPurchaseReceiptLineApiClient
{
    private const string Base = "api/v1/procurement/purchase-receipt-lines";

    public PurchaseReceiptLineApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<PurchaseReceiptLineListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<PurchaseReceiptLineListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<PurchaseReceiptLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<PurchaseReceiptLineDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<PurchaseReceiptLineLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<PurchaseReceiptLineLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreatePurchaseReceiptLineRequest request, CancellationToken ct = default)
        => PostAsync<CreatePurchaseReceiptLineRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePurchaseReceiptLineRequest request, CancellationToken ct = default)
        => PutAsync<UpdatePurchaseReceiptLineRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
