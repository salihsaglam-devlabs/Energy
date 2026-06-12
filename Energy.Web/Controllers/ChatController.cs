using Energy.Localization;
using Energy.Shared.Identity.Permissions;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Web.Clients.Chat;
using Energy.Web.Common;
using Energy.Web.Common.Filters;
using Energy.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;

namespace Energy.Web.Controllers;

/// <summary>
/// Chat page is a DevExtreme dxChat shell. Data is proxied to the API (which
/// persists every message), and real-time delivery is layered on top via the
/// SignalR <see cref="ChatHub"/>: after a message is stored it is pushed to the
/// recipient (and the sender's other tabs) so the bell/badge updates live.
/// </summary>
[Authorize]
[PagePermission(PermissionCatalog.ChatUse)]
[Route("chat")]
public sealed class ChatController : Controller
{
    private readonly IChatApiClient _chat;
    private readonly IHubContext<ChatHub> _hub;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public ChatController(IChatApiClient chat, IHubContext<ChatHub> hub, IStringLocalizer<SharedResource> localizer)
    {
        _chat = chat;
        _hub = hub;
        _localizer = localizer;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        ViewData["Title"] = _localizer.GetText(LocalizationKeys.ChatScreen.Title);

        // Whether the signed-in user has a profile image, so their own messages
        // also render with an avatar in the chat window. Uses the chat-scoped
        // avatar endpoint so no profile/user-management permission is needed.
        var userId = User.GetUserId();
        var hasImage = false;
        if (userId is { } id)
        {
            var (content, _, status) = await _chat.GetUserAvatarAsync(id, ct);
            hasImage = status == 200 && content.Length > 0;
        }
        ViewData["UserHasImage"] = hasImage;

        return View();
    }

    [HttpGet("contacts")]
    public async Task<IActionResult> Contacts(CancellationToken ct)
    {
        var envelope = await _chat.GetContactsAsync(ct);
        return Json(envelope.Data ?? Array.Empty<Shared.Models.V1.Chat.Responses.ChatContactResponse>());
    }

    [HttpGet("conversation/{peerId:guid}")]
    public async Task<IActionResult> Conversation(Guid peerId, CancellationToken ct)
    {
        var envelope = await _chat.GetConversationAsync(peerId, ct);
        return Json(envelope.Data ?? Array.Empty<Shared.Models.V1.Chat.Responses.ChatMessageResponse>());
    }

    /// <summary>Streams a user's profile image so the chat can render it as an avatar.</summary>
    [HttpGet("avatar/{userId:guid}")]
    public async Task<IActionResult> Avatar(Guid userId, CancellationToken ct)
    {
        // Served via the chat-scoped API endpoint (ChatUse) so peers' avatars
        // load even for users without the user-management/profile permissions.
        var (content, contentType, status) = await _chat.GetUserAvatarAsync(userId, ct);
        if (status != 200 || content.Length == 0)
        {
            return NotFound();
        }
        return File(content, string.IsNullOrWhiteSpace(contentType) ? "image/png" : contentType);
    }

    /// <summary>Streams the file shared inside a chat message (participants only; enforced by the API).</summary>
    [HttpGet("messages/{messageId:guid}/attachment")]
    public async Task<IActionResult> Attachment(Guid messageId, CancellationToken ct)
    {
        var (content, contentType, status) = await _chat.GetAttachmentAsync(messageId, ct);
        if (status != 200 || content.Length == 0)
        {
            return NotFound();
        }
        return File(content, string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType);
    }

    public sealed class SendInput
    {
        public Guid? RecipientId { get; set; }
        public Guid? GroupId { get; set; }
        public string Text { get; set; } = string.Empty;
        public string? AttachmentFileName { get; set; }
        public string? AttachmentContentType { get; set; }
        public string? AttachmentContentBase64 { get; set; }
    }

    [HttpPost("messages")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Send([FromBody] SendInput input, CancellationToken ct)
    {
        var envelope = await _chat.SendAsync(new SendChatMessageRequest
        {
            RecipientId = input.RecipientId,
            GroupId = input.GroupId,
            Text = input.Text,
            AttachmentFileName = input.AttachmentFileName,
            AttachmentContentType = input.AttachmentContentType,
            AttachmentContentBase64 = input.AttachmentContentBase64
        }, ct);

        var message = envelope.Data;
        if (envelope.IsSuccess && message is not null)
        {
            if (message.GroupId is { } groupId)
            {
                // Fan-out to every accepted group member (including the sender's tabs).
                var memberEnvelope = await _chat.GetGroupMemberIdsAsync(groupId, ct);
                var memberIds = (memberEnvelope.Data ?? Array.Empty<Guid>())
                    .Select(id => id.ToString())
                    .ToArray();
                if (memberIds.Length > 0)
                {
                    await _hub.Clients.Users(memberIds).SendAsync(ChatHub.ReceiveMessage, message, ct);
                }
            }
            else if (message.RecipientId is { } recipientId)
            {
                // Deliver to the recipient (live append + bell) and echo to the
                // sender's other open tabs so every surface stays in sync.
                await _hub.Clients.User(recipientId.ToString()).SendAsync(ChatHub.ReceiveMessage, message, ct);
                await _hub.Clients.User(message.SenderId.ToString()).SendAsync(ChatHub.ReceiveMessage, message, ct);
            }
        }

        return Json(envelope);
    }

    [HttpPost("conversation/{peerId:guid}/read")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> MarkRead(Guid peerId, CancellationToken ct)
    {
        var envelope = await _chat.MarkReadAsync(peerId, ct);

        // Read receipt: tell the peer (the original sender) that we read their
        // messages so their ticks turn to "read".
        var me = User.GetUserId();
        if (envelope.IsSuccess && me is { } readerId)
        {
            await _hub.Clients.User(peerId.ToString()).SendAsync(
                ChatHub.MessagesRead, new { readerId = readerId.ToString() }, ct);
        }

        return Json(envelope);
    }

    [HttpDelete("messages/{messageId:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> DeleteMessage(Guid messageId, CancellationToken ct)
    {
        var envelope = await _chat.DeleteMessageAsync(messageId, ct);
        if (envelope.IsSuccess && envelope.Data is { } m)
        {
            await BroadcastToParticipantsAsync(m, ChatHub.MessageDeleted,
                new { id = m.Id.ToString(), groupId = m.GroupId?.ToString(), peerId = m.RecipientId?.ToString(), senderId = m.SenderId.ToString() }, ct);
        }
        return Json(envelope);
    }

    public sealed class ForwardInput
    {
        public Guid? RecipientId { get; set; }
        public Guid? GroupId { get; set; }
    }

    [HttpPost("messages/{messageId:guid}/forward")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Forward(Guid messageId, [FromBody] ForwardInput input, CancellationToken ct)
    {
        var envelope = await _chat.ForwardAsync(messageId, new Shared.Models.V1.Chat.Requests.ForwardChatMessageRequest
        {
            MessageId = messageId,
            RecipientId = input.RecipientId,
            GroupId = input.GroupId
        }, ct);

        if (envelope.IsSuccess && envelope.Data is { } msg)
        {
            await DeliverMessageAsync(msg, ct);
        }
        return Json(envelope);
    }

    public sealed class ReactInput
    {
        public string Emoji { get; set; } = string.Empty;
    }

    [HttpPost("messages/{messageId:guid}/react")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> React(Guid messageId, [FromBody] ReactInput input, CancellationToken ct)
    {
        var envelope = await _chat.ReactAsync(messageId, new Shared.Models.V1.Chat.Requests.ReactChatMessageRequest
        {
            Emoji = input.Emoji
        }, ct);

        if (envelope.IsSuccess && envelope.Data is { } m)
        {
            await BroadcastToParticipantsAsync(m, ChatHub.MessageReacted, m, ct);
        }
        return Json(envelope);
    }

    // Delivers a (just-created) message to its recipient/group + sender tabs.
    private async Task DeliverMessageAsync(Shared.Models.V1.Chat.Responses.ChatMessageResponse message, CancellationToken ct)
    {
        if (message.GroupId is { } groupId)
        {
            var memberEnvelope = await _chat.GetGroupMemberIdsAsync(groupId, ct);
            var memberIds = (memberEnvelope.Data ?? Array.Empty<Guid>()).Select(id => id.ToString()).ToArray();
            if (memberIds.Length > 0)
            {
                await _hub.Clients.Users(memberIds).SendAsync(ChatHub.ReceiveMessage, message, ct);
            }
        }
        else if (message.RecipientId is { } recipientId)
        {
            await _hub.Clients.User(recipientId.ToString()).SendAsync(ChatHub.ReceiveMessage, message, ct);
            await _hub.Clients.User(message.SenderId.ToString()).SendAsync(ChatHub.ReceiveMessage, message, ct);
        }
    }

    // Sends an arbitrary event/payload to every participant of a message.
    private async Task BroadcastToParticipantsAsync(
        Shared.Models.V1.Chat.Responses.ChatMessageResponse message, string eventName, object payload, CancellationToken ct)
    {
        if (message.GroupId is { } groupId)
        {
            var memberEnvelope = await _chat.GetGroupMemberIdsAsync(groupId, ct);
            var memberIds = (memberEnvelope.Data ?? Array.Empty<Guid>()).Select(id => id.ToString()).ToArray();
            if (memberIds.Length > 0)
            {
                await _hub.Clients.Users(memberIds).SendAsync(eventName, payload, ct);
            }
        }
        else if (message.RecipientId is { } recipientId)
        {
            await _hub.Clients.User(recipientId.ToString()).SendAsync(eventName, payload, ct);
            await _hub.Clients.User(message.SenderId.ToString()).SendAsync(eventName, payload, ct);
        }
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> UnreadCount(CancellationToken ct)
    {
        var envelope = await _chat.GetUnreadCountAsync(ct);
        return Json(new { count = envelope.Data });
    }

    // ----- Groups -----------------------------------------------------------

    [HttpGet("groups")]
    public async Task<IActionResult> Groups(CancellationToken ct)
    {
        var envelope = await _chat.GetGroupsAsync(ct);
        return Json(envelope.Data ?? Array.Empty<Shared.Models.V1.Chat.Responses.ChatGroupResponse>());
    }

    [HttpGet("groups/invites")]
    public async Task<IActionResult> GroupInvites(CancellationToken ct)
    {
        var envelope = await _chat.GetGroupInvitesAsync(ct);
        return Json(envelope.Data ?? Array.Empty<Shared.Models.V1.Chat.Responses.ChatGroupInviteResponse>());
    }

    [HttpGet("groups/{groupId:guid}/members")]
    public async Task<IActionResult> GroupMembers(Guid groupId, CancellationToken ct)
    {
        var envelope = await _chat.GetGroupMembersAsync(groupId, ct);
        return Json(envelope.Data ?? Array.Empty<Shared.Models.V1.Chat.Responses.ChatGroupMemberResponse>());
    }

    [HttpGet("groups/{groupId:guid}/conversation")]
    public async Task<IActionResult> GroupConversation(Guid groupId, CancellationToken ct)
    {
        var envelope = await _chat.GetGroupConversationAsync(groupId, ct);
        return Json(envelope.Data ?? Array.Empty<Shared.Models.V1.Chat.Responses.ChatMessageResponse>());
    }

    public sealed class CreateGroupInput
    {
        public string Name { get; set; } = string.Empty;
        public List<Guid> MemberUserIds { get; set; } = new();
    }

    [HttpPost("groups")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupInput input, CancellationToken ct)
    {
        var envelope = await _chat.CreateGroupAsync(new Shared.Models.V1.Chat.Requests.CreateChatGroupRequest
        {
            Name = input.Name,
            MemberUserIds = input.MemberUserIds ?? new List<Guid>()
        }, ct);

        if (envelope.IsSuccess && envelope.Data is { } group && input.MemberUserIds is { Count: > 0 })
        {
            await NotifyInviteesAsync(input.MemberUserIds, group.Id, group.Name, ct);
        }

        return Json(envelope);
    }

    public sealed class InviteInput
    {
        public List<Guid> UserIds { get; set; } = new();
    }

    [HttpPost("groups/{groupId:guid}/invite")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> InviteToGroup(Guid groupId, [FromBody] InviteInput input, CancellationToken ct)
    {
        var envelope = await _chat.InviteToGroupAsync(groupId, new Shared.Models.V1.Chat.Requests.InviteToGroupRequest
        {
            UserIds = input.UserIds ?? new List<Guid>()
        }, ct);

        if (envelope.IsSuccess && envelope.Data is { Count: > 0 } invited)
        {
            await NotifyInviteesAsync(invited.ToList(), groupId, string.Empty, ct);
        }

        return Json(envelope);
    }

    public sealed class RespondInput
    {
        public bool Accept { get; set; }
    }

    [HttpPost("groups/{groupId:guid}/respond")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> RespondInvite(Guid groupId, [FromBody] RespondInput input, CancellationToken ct)
    {
        var envelope = await _chat.RespondInviteAsync(groupId, new Shared.Models.V1.Chat.Requests.RespondGroupInviteRequest
        {
            Accept = input.Accept
        }, ct);

        if (envelope.IsSuccess && envelope.Data)
        {
            // Tell existing members the roster changed (refresh members/groups).
            var memberEnvelope = await _chat.GetGroupMemberIdsAsync(groupId, ct);
            var memberIds = (memberEnvelope.Data ?? Array.Empty<Guid>()).Select(id => id.ToString()).ToArray();
            if (memberIds.Length > 0)
            {
                await _hub.Clients.Users(memberIds).SendAsync(ChatHub.GroupChanged, new { groupId = groupId.ToString() }, ct);
            }
        }

        return Json(envelope);
    }

    // Pushes a "you've been invited" event to each invitee's open tabs.
    private async Task NotifyInviteesAsync(IEnumerable<Guid> userIds, Guid groupId, string groupName, CancellationToken ct)
    {
        var ids = userIds.Select(id => id.ToString()).ToArray();
        if (ids.Length == 0) { return; }
        await _hub.Clients.Users(ids).SendAsync(
            ChatHub.GroupInvite,
            new { groupId = groupId.ToString(), groupName },
            ct);
    }
}

