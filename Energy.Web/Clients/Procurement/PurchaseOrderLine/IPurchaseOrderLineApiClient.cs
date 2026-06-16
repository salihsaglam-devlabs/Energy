using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Procurement.PurchaseOrderLine;

/// <summary>PurchaseOrderLine API istemci sözleşmesi.</summary>
public interface IPurchaseOrderLineApiClient
{
    Task<BaseResponse<PaginatedResponse<PurchaseOrderLineListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<PurchaseOrderLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<PurchaseOrderLineLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreatePurchaseOrderLineRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePurchaseOrderLineRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>PurchaseOrderLine API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class PurchaseOrderLineApiClient : ApiClientBase, IPurchaseOrderLineApiClient
{
    private const string Base = "api/v1/procurement/purchase-order-lines";

    public PurchaseOrderLineApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<PurchaseOrderLineListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<PurchaseOrderLineListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<PurchaseOrderLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<PurchaseOrderLineDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<PurchaseOrderLineLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<PurchaseOrderLineLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreatePurchaseOrderLineRequest request, CancellationToken ct = default)
        => PostAsync<CreatePurchaseOrderLineRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePurchaseOrderLineRequest request, CancellationToken ct = default)
        => PutAsync<UpdatePurchaseOrderLineRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
