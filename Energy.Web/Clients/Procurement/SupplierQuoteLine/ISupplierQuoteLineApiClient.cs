using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Requests;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Procurement.SupplierQuoteLine;

/// <summary>SupplierQuoteLine API istemci sözleşmesi.</summary>
public interface ISupplierQuoteLineApiClient
{
    Task<BaseResponse<PaginatedResponse<SupplierQuoteLineListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<SupplierQuoteLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<SupplierQuoteLineLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateSupplierQuoteLineRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSupplierQuoteLineRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>SupplierQuoteLine API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class SupplierQuoteLineApiClient : ApiClientBase, ISupplierQuoteLineApiClient
{
    private const string Base = "api/v1/procurement/supplier-quote-lines";

    public SupplierQuoteLineApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<SupplierQuoteLineListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<SupplierQuoteLineListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<SupplierQuoteLineDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<SupplierQuoteLineDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<SupplierQuoteLineLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<SupplierQuoteLineLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateSupplierQuoteLineRequest request, CancellationToken ct = default)
        => PostAsync<CreateSupplierQuoteLineRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSupplierQuoteLineRequest request, CancellationToken ct = default)
        => PutAsync<UpdateSupplierQuoteLineRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
