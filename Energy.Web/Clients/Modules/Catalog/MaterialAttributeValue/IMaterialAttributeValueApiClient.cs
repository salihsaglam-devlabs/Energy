using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Requests;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeValue.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Modules.Catalog.MaterialAttributeValue;

/// <summary>MaterialAttributeValue API istemci sözleşmesi.</summary>
public interface IMaterialAttributeValueApiClient
{
    Task<BaseResponse<PaginatedResponse<MaterialAttributeValueListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<MaterialAttributeValueDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<MaterialAttributeValueLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateMaterialAttributeValueRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialAttributeValueRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>MaterialAttributeValue API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class MaterialAttributeValueApiClient : ApiClientBase, IMaterialAttributeValueApiClient
{
    private const string Base = "api/v1/catalog/material-attribute-values";

    public MaterialAttributeValueApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<MaterialAttributeValueListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<MaterialAttributeValueListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<MaterialAttributeValueDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<MaterialAttributeValueDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<MaterialAttributeValueLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<MaterialAttributeValueLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateMaterialAttributeValueRequest request, CancellationToken ct = default)
        => PostAsync<CreateMaterialAttributeValueRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateMaterialAttributeValueRequest request, CancellationToken ct = default)
        => PutAsync<UpdateMaterialAttributeValueRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
