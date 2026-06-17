using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialCategoryAttribute.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Catalog.MaterialCategoryAttribute;

/// <summary>MaterialCategoryAttribute API istemci sözleşmesi.</summary>
public interface IMaterialCategoryAttributeApiClient
{
    Task<BaseResponse<PaginatedResponse<MaterialCategoryAttributeListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<MaterialCategoryAttributeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<MaterialCategoryAttributeLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateMaterialCategoryAttributeRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialCategoryAttributeRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>MaterialCategoryAttribute API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class MaterialCategoryAttributeApiClient : ApiClientBase, IMaterialCategoryAttributeApiClient
{
    private const string Base = "api/v1/catalog/material-category-attributes";

    public MaterialCategoryAttributeApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<MaterialCategoryAttributeListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<MaterialCategoryAttributeListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<MaterialCategoryAttributeDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<MaterialCategoryAttributeDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<MaterialCategoryAttributeLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<MaterialCategoryAttributeLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateMaterialCategoryAttributeRequest request, CancellationToken ct = default)
        => PostAsync<CreateMaterialCategoryAttributeRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialCategoryAttributeRequest request, CancellationToken ct = default)
        => PutAsync<UpdateMaterialCategoryAttributeRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
