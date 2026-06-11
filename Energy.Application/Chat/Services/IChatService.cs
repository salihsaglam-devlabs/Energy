using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;

namespace Energy.Application.Chat.Services;

/// <summary>
/// Direct (1-to-1) messaging between users. Every message is persisted; the
/// real-time delivery is handled by the Web layer's SignalR hub on top of the
/// values returned here.
/// </summary>
public interface IChatService
{
    /// <summary>Every other active user, with the current user's unread count from each.</summary>
    Task<IReadOnlyList<ChatContactResponse>> GetContactsAsync(Guid currentUserId, CancellationToken cancellationToken = default);

    /// <summary>Ordered message history between the current user and a peer.</summary>
    Task<IReadOnlyList<ChatMessageResponse>> GetConversationAsync(Guid currentUserId, Guid peerId, CancellationToken cancellationToken = default);

    /// <summary>Persists a message from the current user and returns the stored row.</summary>
    Task<ChatMessageResponse> SendAsync(Guid senderId, SendChatMessageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the binary attachment of a message, but only if the current user is a
    /// participant (sender or recipient) of that message. Returns <c>null</c> otherwise.
    /// </summary>
    Task<ChatAttachmentResponse?> GetAttachmentAsync(Guid currentUserId, Guid messageId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a user's profile image for rendering chat avatars. Scoped to chat
    /// (any participant may see a peer's avatar) so it does not require the
    /// user-management/profile permissions the Users endpoints demand.
    /// </summary>
    Task<ChatAttachmentResponse?> GetUserAvatarAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>Marks every message from <paramref name="peerId"/> to the current user as read.</summary>
    Task<int> MarkReadAsync(Guid currentUserId, Guid peerId, CancellationToken cancellationToken = default);

    /// <summary>Total unread messages addressed to the current user (drives the global bell).</summary>
    Task<int> GetUnreadCountAsync(Guid currentUserId, CancellationToken cancellationToken = default);
}

