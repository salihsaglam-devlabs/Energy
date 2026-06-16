using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.LocalizationResource.Requests;
using Energy.Shared.Models.V1.Core.LocalizationResource.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Core.LocalizationResource;

/// <summary>LocalizationResource API istemci sözleşmesi.</summary>
public interface ILocalizationResourceApiClient
{
    Task<BaseResponse<PaginatedResponse<LocalizationResourceListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<LocalizationResourceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<LocalizationResourceLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateLocalizationResourceRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateLocalizationResourceRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>LocalizationResource API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class LocalizationResourceApiClient : ApiClientBase, ILocalizationResourceApiClient
{
    private const string Base = "api/v1/core/localization-resources";

    public LocalizationResourceApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<LocalizationResourceListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<LocalizationResourceListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<LocalizationResourceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<LocalizationResourceDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<LocalizationResourceLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<LocalizationResourceLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateLocalizationResourceRequest request, CancellationToken ct = default)
        => PostAsync<CreateLocalizationResourceRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateLocalizationResourceRequest request, CancellationToken ct = default)
        => PutAsync<UpdateLocalizationResourceRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
