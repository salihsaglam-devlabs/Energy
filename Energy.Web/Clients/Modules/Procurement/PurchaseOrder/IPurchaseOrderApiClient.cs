using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Requests;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.Procurement.PurchaseOrder;

/// <summary>PurchaseOrder API istemci sözleşmesi.</summary>
public interface IPurchaseOrderApiClient
{
    Task<BaseResponse<PaginatedResponse<PurchaseOrderListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<PurchaseOrderDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<PurchaseOrderLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreatePurchaseOrderRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePurchaseOrderRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>PurchaseOrder API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class PurchaseOrderApiClient : ApiClientBase, IPurchaseOrderApiClient
{
    private const string Base = "api/v1/procurement/purchase-orders";

    public PurchaseOrderApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<PurchaseOrderListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<PurchaseOrderListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<PurchaseOrderDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<PurchaseOrderDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<PurchaseOrderLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<PurchaseOrderLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreatePurchaseOrderRequest request, CancellationToken ct = default)
        => PostAsync<CreatePurchaseOrderRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdatePurchaseOrderRequest request, CancellationToken ct = default)
        => PutAsync<UpdatePurchaseOrderRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
