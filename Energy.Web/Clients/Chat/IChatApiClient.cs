using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Shared.Models.V1.Common.Responses;

namespace Energy.Web.Clients.Chat;

public interface IChatApiClient
{
    Task<BaseResponse<IReadOnlyList<ChatContactResponse>>> GetContactsAsync(CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ChatMessageResponse>>> GetConversationAsync(Guid peerId, CancellationToken ct = default);
    Task<BaseResponse<ChatMessageResponse>> SendAsync(SendChatMessageRequest request, CancellationToken ct = default);
    Task<(byte[] Content, string ContentType, int StatusCode)> GetAttachmentAsync(Guid messageId, CancellationToken ct = default);
    Task<(byte[] Content, string ContentType, int StatusCode)> GetUserAvatarAsync(Guid userId, CancellationToken ct = default);
    Task<BaseResponse<int>> MarkReadAsync(Guid peerId, CancellationToken ct = default);
    Task<BaseResponse<int>> GetUnreadCountAsync(CancellationToken ct = default);
}

