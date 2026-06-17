using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Requests;
using Energy.Shared.Models.V1.Notifications.NotificationPreference.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Notifications.NotificationPreference;

/// <summary>NotificationPreference API istemci sözleşmesi.</summary>
public interface INotificationPreferenceApiClient
{
    Task<BaseResponse<PaginatedResponse<NotificationPreferenceListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<NotificationPreferenceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<NotificationPreferenceLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateNotificationPreferenceRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateNotificationPreferenceRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>NotificationPreference API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class NotificationPreferenceApiClient : ApiClientBase, INotificationPreferenceApiClient
{
    private const string Base = "api/v1/notifications/notification-preferences";

    public NotificationPreferenceApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<NotificationPreferenceListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<NotificationPreferenceListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<NotificationPreferenceDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<NotificationPreferenceDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<NotificationPreferenceLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<NotificationPreferenceLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateNotificationPreferenceRequest request, CancellationToken ct = default)
        => PostAsync<CreateNotificationPreferenceRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateNotificationPreferenceRequest request, CancellationToken ct = default)
        => PutAsync<UpdateNotificationPreferenceRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
