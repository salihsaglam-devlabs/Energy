using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.Notification.Requests;
using Energy.Shared.Models.V1.Notifications.Notification.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Notifications.Notification;

/// <summary>Notification API istemci sözleşmesi.</summary>
public interface INotificationApiClient
{
    Task<BaseResponse<PaginatedResponse<NotificationListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<NotificationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<NotificationLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateNotificationRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateNotificationRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>Notification API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class NotificationApiClient : ApiClientBase, INotificationApiClient
{
    private const string Base = "api/v1/notifications/notifications";

    public NotificationApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<NotificationListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<NotificationListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<NotificationDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<NotificationDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<NotificationLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<NotificationLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateNotificationRequest request, CancellationToken ct = default)
        => PostAsync<CreateNotificationRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateNotificationRequest request, CancellationToken ct = default)
        => PutAsync<UpdateNotificationRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
