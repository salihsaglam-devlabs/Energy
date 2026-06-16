using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Requests;
using Energy.Shared.Models.V1.Notifications.NotificationRecipient.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Notifications.NotificationRecipient;

/// <summary>NotificationRecipient API istemci sözleşmesi.</summary>
public interface INotificationRecipientApiClient
{
    Task<BaseResponse<PaginatedResponse<NotificationRecipientListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default);
    Task<BaseResponse<NotificationRecipientDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<NotificationRecipientLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default);
    Task<BaseResponse<Guid>> CreateAsync(CreateNotificationRecipientRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateNotificationRecipientRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);
}

/// <summary>NotificationRecipient API istemcisi (HttpClientFactory + BaseResponse).</summary>
public sealed class NotificationRecipientApiClient : ApiClientBase, INotificationRecipientApiClient
{
    private const string Base = "api/v1/notifications/notification-recipients";

    public NotificationRecipientApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<PaginatedResponse<NotificationRecipientListResponse>>> GetListAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        => GetAsync<BaseResponse<PaginatedResponse<NotificationRecipientListResponse>>>($"{Base}?pageNumber={pageNumber}&pageSize={pageSize}&search={Uri.EscapeDataString(search ?? string.Empty)}", ct);

    public Task<BaseResponse<NotificationRecipientDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        => GetAsync<BaseResponse<NotificationRecipientDetailResponse>>($"{Base}/{id}", ct);

    public Task<BaseResponse<IReadOnlyList<NotificationRecipientLookupResponse>>> GetLookupAsync(string? search, bool activeOnly, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<NotificationRecipientLookupResponse>>>($"{Base}/lookup?search={Uri.EscapeDataString(search ?? string.Empty)}&activeOnly={activeOnly}", ct);

    public Task<BaseResponse<Guid>> CreateAsync(CreateNotificationRecipientRequest request, CancellationToken ct = default)
        => PostAsync<CreateNotificationRecipientRequest, BaseResponse<Guid>>(Base, request, ct);

    public Task<BaseResponse<bool>> UpdateAsync(Guid id, UpdateNotificationRecipientRequest request, CancellationToken ct = default)
        => PutAsync<UpdateNotificationRecipientRequest, BaseResponse<bool>>($"{Base}/{id}", request, ct);

    public Task<BaseResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>($"{Base}/{id}", ct);
}
