using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialUnitConversion.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Catalog.MaterialUnitConversion;

/// <summary>MaterialUnitConversion API istemci sözleşmesi.</summary>
public interface IMaterialUnitConversionApiClient
{
    Task<BaseResponse<PaginatedResponse<MaterialUnitConversionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<MaterialUnitConversionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<MaterialUnitConversionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateMaterialUnitConversionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialUnitConversionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>MaterialUnitConversion API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class MaterialUnitConversionApiClient : ApiClientBase, IMaterialUnitConversionApiClient
{
    private const string Base = "api/v1/catalog/material-unit-conversions";

    public MaterialUnitConversionApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<MaterialUnitConversionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<MaterialUnitConversionListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<MaterialUnitConversionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<MaterialUnitConversionDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<MaterialUnitConversionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<MaterialUnitConversionLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateMaterialUnitConversionRequest request, CancellationToken ct = default)
        => PostAsync<CreateMaterialUnitConversionRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialUnitConversionRequest request, CancellationToken ct = default)
        => PutAsync<UpdateMaterialUnitConversionRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
