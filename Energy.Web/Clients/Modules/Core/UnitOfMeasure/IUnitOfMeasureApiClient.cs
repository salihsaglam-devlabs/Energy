using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Requests;
using Energy.Shared.Models.V1.Core.UnitOfMeasure.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.Core.UnitOfMeasure;

/// <summary>UnitOfMeasure API istemci sözleşmesi.</summary>
public interface IUnitOfMeasureApiClient
{
    Task<BaseResponse<PaginatedResponse<UnitOfMeasureListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<UnitOfMeasureDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<UnitOfMeasureLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateUnitOfMeasureRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUnitOfMeasureRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>UnitOfMeasure API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class UnitOfMeasureApiClient : ApiClientBase, IUnitOfMeasureApiClient
{
    private const string Base = "api/v1/core/units-of-measure";

    public UnitOfMeasureApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<UnitOfMeasureListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<UnitOfMeasureListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<UnitOfMeasureDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<UnitOfMeasureDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<UnitOfMeasureLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<UnitOfMeasureLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateUnitOfMeasureRequest request, CancellationToken ct = default)
        => PostAsync<CreateUnitOfMeasureRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateUnitOfMeasureRequest request, CancellationToken ct = default)
        => PutAsync<UpdateUnitOfMeasureRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
