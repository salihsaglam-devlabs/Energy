using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Procurement.SupplierInvoice;

/// <summary>SupplierInvoice API istemci sözleşmesi.</summary>
public interface ISupplierInvoiceApiClient
{
    Task<BaseResponse<PaginatedResponse<SupplierInvoiceListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<SupplierInvoiceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<SupplierInvoiceLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateSupplierInvoiceRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSupplierInvoiceRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>SupplierInvoice API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class SupplierInvoiceApiClient : ApiClientBase, ISupplierInvoiceApiClient
{
    private const string Base = "api/v1/procurement/supplier-invoices";

    public SupplierInvoiceApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<SupplierInvoiceListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<SupplierInvoiceListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<SupplierInvoiceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<SupplierInvoiceDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<SupplierInvoiceLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<SupplierInvoiceLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateSupplierInvoiceRequest request, CancellationToken ct = default)
        => PostAsync<CreateSupplierInvoiceRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSupplierInvoiceRequest request, CancellationToken ct = default)
        => PutAsync<UpdateSupplierInvoiceRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
