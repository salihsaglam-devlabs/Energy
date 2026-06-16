using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockReservation.Requests;
using Energy.Shared.Models.V1.Inventory.StockReservation.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Inventory.StockReservation;

/// <summary>StockReservation API istemci sözleşmesi.</summary>
public interface IStockReservationApiClient
{
    Task<BaseResponse<PaginatedResponse<StockReservationListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<StockReservationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<StockReservationLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateStockReservationRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockReservationRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>StockReservation API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class StockReservationApiClient : ApiClientBase, IStockReservationApiClient
{
    private const string Base = "api/v1/inventory/stock-reservations";

    public StockReservationApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<StockReservationListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<StockReservationListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<StockReservationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<StockReservationDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<StockReservationLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<StockReservationLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateStockReservationRequest request, CancellationToken ct = default)
        => PostAsync<CreateStockReservationRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateStockReservationRequest request, CancellationToken ct = default)
        => PutAsync<UpdateStockReservationRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
