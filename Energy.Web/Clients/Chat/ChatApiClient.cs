using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Chat;

public sealed class ChatApiClient : ApiClientBase, IChatApiClient
{
    public ChatApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<IReadOnlyList<ChatContactResponse>>> GetContactsAsync(CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ChatContactResponse>>>(ApiRoutes.Chat.Contacts, ct);

    public Task<BaseResponse<IReadOnlyList<ChatMessageResponse>>> GetConversationAsync(Guid peerId, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ChatMessageResponse>>>(ApiRoutes.Chat.Conversation(peerId), ct);

    public Task<BaseResponse<ChatMessageResponse>> SendAsync(SendChatMessageRequest request, CancellationToken ct = default)
        => PostAsync<SendChatMessageRequest, BaseResponse<ChatMessageResponse>>(ApiRoutes.Chat.Messages, request, ct);

    public Task<(byte[] Content, string ContentType, int StatusCode)> GetAttachmentAsync(Guid messageId, CancellationToken ct = default)
        => GetRawAsync(ApiRoutes.Chat.MessageAttachment(messageId), ct);

    public Task<(byte[] Content, string ContentType, int StatusCode)> GetUserAvatarAsync(Guid userId, CancellationToken ct = default)
        => GetRawAsync(ApiRoutes.Chat.UserAvatar(userId), ct);

    public Task<BaseResponse<int>> MarkReadAsync(Guid peerId, CancellationToken ct = default)
        => PostAsync<BaseResponse<int>>(ApiRoutes.Chat.MarkRead(peerId), ct);

    public Task<BaseResponse<int>> GetUnreadCountAsync(CancellationToken ct = default)
        => GetAsync<BaseResponse<int>>(ApiRoutes.Chat.UnreadCount, ct);
}

