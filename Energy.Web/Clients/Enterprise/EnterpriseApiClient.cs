using System.Text.Json;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Versioning;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Enterprise;

/// <summary>
/// <see cref="IEnterpriseApiClient"/>'in HTTP uygulaması. Kimlik doğrulama ve istemci
/// kimliği başlıkları DelegatingHandler zinciri tarafından eklenir.
/// </summary>
public sealed class EnterpriseApiClient : ApiClientBase, IEnterpriseApiClient
{
    private static readonly string V1 = $"api/v{ApiVersions.V1UrlSegment}";

    public EnterpriseApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<JsonElement>>> ListAsync(
        string module, int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<JsonElement>>>(
            $"{V1}/{module}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<JsonElement>> GetByIdAsync(string module, Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<JsonElement>>($"{V1}/{module}/{id}", ct);

    public Task<BaseResponse<JsonElement>> CreateAsync(string module, JsonElement body, CancellationToken ct = default)
        => PostAsync<JsonElement, BaseResponse<JsonElement>>($"{V1}/{module}", body, ct);

    public Task<BaseResponse<JsonElement>> UpdateAsync(string module, Guid id, JsonElement body, CancellationToken ct = default)
        => PutAsync<JsonElement, BaseResponse<JsonElement>>($"{V1}/{module}/{id}", body, ct);

    public Task<BaseResponse<bool>> DeleteAsync(string module, Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{V1}/{module}/{id}", ct);

    public Task<BaseResponse<JsonElement>> PostActionAsync(string apiRelativePath, JsonElement? body, CancellationToken ct = default)
        => body is { } payload
            ? PostAsync<JsonElement, BaseResponse<JsonElement>>($"{V1}/{apiRelativePath}", payload, ct)
            : PostAsync<BaseResponse<JsonElement>>($"{V1}/{apiRelativePath}", ct);

    public Task<BaseResponse<JsonElement>> GetActionAsync(string apiRelativePath, CancellationToken ct = default)
        => GetAsync<BaseResponse<JsonElement>>($"{V1}/{apiRelativePath}", ct);

    public Task<BaseResponse<PaginatedResponse<JsonElement>>> ListChildrenAsync(
        string detailKey, Guid parentId, int pageNumber, int pageSize, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<JsonElement>>>(
            $"{V1}/details/{detailKey}?parentId={parentId}&pageNumber={pageNumber}&pageSize={pageSize}", ct);

    public Task<BaseResponse<JsonElement>> CreateChildAsync(string detailKey, Guid parentId, JsonElement body, CancellationToken ct = default)
        => PostAsync<JsonElement, BaseResponse<JsonElement>>($"{V1}/details/{detailKey}?parentId={parentId}", body, ct);

    public Task<BaseResponse<JsonElement>> UpdateChildAsync(string detailKey, Guid id, JsonElement body, CancellationToken ct = default)
        => PutAsync<JsonElement, BaseResponse<JsonElement>>($"{V1}/details/{detailKey}/{id}", body, ct);

    public Task<BaseResponse<bool>> DeleteChildAsync(string detailKey, Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{V1}/details/{detailKey}/{id}", ct);
}

