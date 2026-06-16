using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Catalog.MaterialAttributeOption;

/// <summary>MaterialAttributeOption API istemci sözleşmesi.</summary>
public interface IMaterialAttributeOptionApiClient
{
    Task<BaseResponse<PaginatedResponse<MaterialAttributeOptionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<MaterialAttributeOptionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<MaterialAttributeOptionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateMaterialAttributeOptionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialAttributeOptionRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>MaterialAttributeOption API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class MaterialAttributeOptionApiClient : ApiClientBase, IMaterialAttributeOptionApiClient
{
    private const string Base = "api/v1/catalog/material-attribute-options";

    public MaterialAttributeOptionApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<MaterialAttributeOptionListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<MaterialAttributeOptionListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<MaterialAttributeOptionDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<MaterialAttributeOptionDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<MaterialAttributeOptionLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<MaterialAttributeOptionLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateMaterialAttributeOptionRequest request, CancellationToken ct = default)
        => PostAsync<CreateMaterialAttributeOptionRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialAttributeOptionRequest request, CancellationToken ct = default)
        => PutAsync<UpdateMaterialAttributeOptionRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
