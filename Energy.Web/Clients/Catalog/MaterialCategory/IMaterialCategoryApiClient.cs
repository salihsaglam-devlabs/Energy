using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialCategory.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Catalog.MaterialCategory;

/// <summary>MaterialCategory API istemci sözleşmesi.</summary>
public interface IMaterialCategoryApiClient
{
    Task<BaseResponse<PaginatedResponse<MaterialCategoryListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<MaterialCategoryDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<MaterialCategoryLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateMaterialCategoryRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialCategoryRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>MaterialCategory API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class MaterialCategoryApiClient : ApiClientBase, IMaterialCategoryApiClient
{
    private const string Base = "api/v1/catalog/material-categories";

    public MaterialCategoryApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<MaterialCategoryListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<MaterialCategoryListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<MaterialCategoryDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<MaterialCategoryDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<MaterialCategoryLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<MaterialCategoryLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateMaterialCategoryRequest request, CancellationToken ct = default)
        => PostAsync<CreateMaterialCategoryRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialCategoryRequest request, CancellationToken ct = default)
        => PutAsync<UpdateMaterialCategoryRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
