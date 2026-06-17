using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Procurement.SupplierInvoiceLine;

/// <summary>SupplierInvoiceLine API istemci sözleşmesi.</summary>
public interface ISupplierInvoiceLineApiClient
{
    Task<BaseResponse<PaginatedResponse<SupplierInvoiceLineListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<SupplierInvoiceLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<SupplierInvoiceLineLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateSupplierInvoiceLineRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSupplierInvoiceLineRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>SupplierInvoiceLine API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class SupplierInvoiceLineApiClient : ApiClientBase, ISupplierInvoiceLineApiClient
{
    private const string Base = "api/v1/procurement/supplier-invoice-lines";

    public SupplierInvoiceLineApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<SupplierInvoiceLineListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<SupplierInvoiceLineListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<SupplierInvoiceLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<SupplierInvoiceLineDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<SupplierInvoiceLineLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<SupplierInvoiceLineLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateSupplierInvoiceLineRequest request, CancellationToken ct = default)
        => PostAsync<CreateSupplierInvoiceLineRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSupplierInvoiceLineRequest request, CancellationToken ct = default)
        => PutAsync<UpdateSupplierInvoiceLineRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
