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
    /// Soft-deletes a message for everyone (only the sender may delete). Returns the
    /// deleted message projection (for realtime fan-out) or null if not allowed/found.
    /// </summary>
    Task<ChatMessageResponse?> DeleteMessageAsync(Guid currentUserId, Guid messageId, CancellationToken cancellationToken = default);

    /// <summary>Forwards an existing message's content to a new target. Returns the new message.</summary>
    Task<ChatMessageResponse?> ForwardAsync(Guid currentUserId, ForwardChatMessageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Toggles the current user's emoji reaction on a message (same emoji removes it,
    /// a different emoji replaces it). Returns the updated message projection or null.
    /// </summary>
    Task<ChatMessageResponse?> ToggleReactionAsync(Guid currentUserId, Guid messageId, string emoji, CancellationToken cancellationToken = default);

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

    // ----- Groups -----------------------------------------------------------

    /// <summary>Groups the current user actively belongs to (owner or accepted member).</summary>
    Task<IReadOnlyList<ChatGroupResponse>> GetGroupsAsync(Guid currentUserId, CancellationToken cancellationToken = default);

    /// <summary>Pending group invitations addressed to the current user.</summary>
    Task<IReadOnlyList<ChatGroupInviteResponse>> GetGroupInvitesAsync(Guid currentUserId, CancellationToken cancellationToken = default);

    /// <summary>Creates a group owned by the current user and invites the requested members.</summary>
    Task<ChatGroupResponse> CreateGroupAsync(Guid ownerId, CreateChatGroupRequest request, CancellationToken cancellationToken = default);

    /// <summary>Invites users to a group the current user is a member of. Returns the invited user ids.</summary>
    Task<IReadOnlyList<Guid>> InviteToGroupAsync(Guid currentUserId, Guid groupId, InviteToGroupRequest request, CancellationToken cancellationToken = default);

    /// <summary>Accepts or declines the current user's pending invitation to a group.</summary>
    Task<bool> RespondInviteAsync(Guid currentUserId, Guid groupId, bool accept, CancellationToken cancellationToken = default);

    /// <summary>Members of a group (current user must be an accepted member).</summary>
    Task<IReadOnlyList<ChatGroupMemberResponse>> GetGroupMembersAsync(Guid currentUserId, Guid groupId, CancellationToken cancellationToken = default);

    /// <summary>Accepted member user ids of a group (used to fan-out realtime delivery).</summary>
    Task<IReadOnlyList<Guid>> GetGroupMemberIdsAsync(Guid groupId, CancellationToken cancellationToken = default);

    /// <summary>Ordered message history of a group (current user must be an accepted member).</summary>
    Task<IReadOnlyList<ChatMessageResponse>> GetGroupConversationAsync(Guid currentUserId, Guid groupId, CancellationToken cancellationToken = default);
}

