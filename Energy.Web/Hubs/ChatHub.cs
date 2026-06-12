using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Energy.Web.Hubs;

/// <summary>
/// Real-time chat transport. The MVC <c>ChatController</c> persists messages via
/// the API and then pushes them to the relevant users through this hub using
/// <see cref="IHubContext{ChatHub}"/>. Connections are grouped per user by the
/// default <see cref="IUserIdProvider"/> (NameIdentifier claim), so a user
/// receives notifications on every open tab regardless of the current page.
/// On top of message delivery the hub also tracks online/offline presence and
/// relays typing indicators between conversation peers.
/// </summary>
[Authorize]
public sealed class ChatHub : Hub
{
    /// <summary>Server → client event names.</summary>
    public const string ReceiveMessage = "ReceiveMessage";
    public const string UnreadCountChanged = "UnreadCountChanged";
    public const string PresenceChanged = "PresenceChanged";
    public const string PresenceSnapshot = "PresenceSnapshot";
    public const string TypingChanged = "TypingChanged";

    /// <summary>A user was invited to a group (delivered to the invitee).</summary>
    public const string GroupInvite = "GroupInvite";

    /// <summary>A group's membership/state changed (delivered to members).</summary>
    public const string GroupChanged = "GroupChanged";

    /// <summary>A message was deleted for everyone.</summary>
    public const string MessageDeleted = "MessageDeleted";

    /// <summary>A message's reactions changed.</summary>
    public const string MessageReacted = "MessageReacted";

    /// <summary>The peer read the current user's messages (read receipts).</summary>
    public const string MessagesRead = "MessagesRead";

    // Voice-call (WebRTC) signaling events (server → client).
    public const string CallOffer = "CallOffer";
    public const string CallAnswered = "CallAnswered";
    public const string CallIce = "CallIce";
    public const string CallEnded = "CallEnded";

    private readonly IChatPresenceTracker _presence;

    public ChatHub(IChatPresenceTracker presence)
    {
        _presence = presence;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = CurrentUserId;
        if (userId is { } id)
        {
            var becameOnline = _presence.Add(id, Context.ConnectionId);

            // Give the freshly connected client the full online roster so it can
            // paint presence tags immediately.
            await Clients.Caller.SendAsync(
                PresenceSnapshot,
                _presence.OnlineUsers.Select(u => u.ToString()).ToArray());

            if (becameOnline)
            {
                await Clients.Others.SendAsync(
                    PresenceChanged,
                    new { userId = id.ToString(), isOnline = true });
            }
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = CurrentUserId;
        if (userId is { } id)
        {
            var becameOffline = _presence.Remove(id, Context.ConnectionId);
            if (becameOffline)
            {
                await Clients.Others.SendAsync(
                    PresenceChanged,
                    new { userId = id.ToString(), isOnline = false });
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>Relays a typing indicator from the current user to a single peer.</summary>
    public Task Typing(string recipientId, bool isTyping)
    {
        var userId = CurrentUserId;
        if (userId is null || string.IsNullOrWhiteSpace(recipientId))
        {
            return Task.CompletedTask;
        }

        return Clients.User(recipientId).SendAsync(
            TypingChanged,
            new { fromUserId = userId.Value.ToString(), isTyping });
    }

    /// <summary>
    /// Lets a client (re)request the current online roster — used right after
    /// (re)connecting so presence is always fresh without waiting for the next
    /// connect/disconnect event.
    /// </summary>
    public Task RequestPresence()
        => Clients.Caller.SendAsync(
            PresenceSnapshot,
            _presence.OnlineUsers.Select(u => u.ToString()).ToArray());

    // ----- Voice call (WebRTC) signaling. Each method relays the SDP/ICE to the
    // target user, tagging the payload with the caller's identity. -------------

    public Task CallUser(string targetUserId, string callerName, object offer)
    {
        var userId = CurrentUserId;
        if (userId is null || string.IsNullOrWhiteSpace(targetUserId)) { return Task.CompletedTask; }
        return Clients.User(targetUserId).SendAsync(CallOffer,
            new { fromUserId = userId.Value.ToString(), callerName, offer });
    }

    public Task AnswerCall(string targetUserId, object answer)
    {
        var userId = CurrentUserId;
        if (userId is null || string.IsNullOrWhiteSpace(targetUserId)) { return Task.CompletedTask; }
        return Clients.User(targetUserId).SendAsync(CallAnswered,
            new { fromUserId = userId.Value.ToString(), answer });
    }

    public Task SendIce(string targetUserId, object candidate)
    {
        var userId = CurrentUserId;
        if (userId is null || string.IsNullOrWhiteSpace(targetUserId)) { return Task.CompletedTask; }
        return Clients.User(targetUserId).SendAsync(CallIce,
            new { fromUserId = userId.Value.ToString(), candidate });
    }

    public Task EndCall(string targetUserId)
    {
        var userId = CurrentUserId;
        if (userId is null || string.IsNullOrWhiteSpace(targetUserId)) { return Task.CompletedTask; }
        return Clients.User(targetUserId).SendAsync(CallEnded,
            new { fromUserId = userId.Value.ToString() });
    }

    private Guid? CurrentUserId
        => Guid.TryParse(Context.UserIdentifier, out var id) ? id : null;
}

