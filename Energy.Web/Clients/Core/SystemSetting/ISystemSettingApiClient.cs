using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Core.SystemSetting.Requests;
using Energy.Shared.Models.V1.Core.SystemSetting.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Core.SystemSetting;

/// <summary>SystemSetting API istemci sözleşmesi.</summary>
public interface ISystemSettingApiClient
{
    Task<BaseResponse<PaginatedResponse<SystemSettingListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<SystemSettingDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<SystemSettingLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateSystemSettingRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSystemSettingRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>SystemSetting API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class SystemSettingApiClient : ApiClientBase, ISystemSettingApiClient
{
    private const string Base = "api/v1/core/system-settings";

    public SystemSettingApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<SystemSettingListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<SystemSettingListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<SystemSettingDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<SystemSettingDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<SystemSettingLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<SystemSettingLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateSystemSettingRequest request, CancellationToken ct = default)
        => PostAsync<CreateSystemSettingRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateSystemSettingRequest request, CancellationToken ct = default)
        => PutAsync<UpdateSystemSettingRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
