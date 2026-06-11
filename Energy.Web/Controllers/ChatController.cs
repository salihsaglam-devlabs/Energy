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
        public Guid RecipientId { get; set; }
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
            Text = input.Text,
            AttachmentFileName = input.AttachmentFileName,
            AttachmentContentType = input.AttachmentContentType,
            AttachmentContentBase64 = input.AttachmentContentBase64
        }, ct);

        var message = envelope.Data;
        if (envelope.IsSuccess && message is not null)
        {
            // Deliver to the recipient (live append + bell) and echo to the
            // sender's other open tabs so every surface stays in sync.
            await _hub.Clients.User(message.RecipientId.ToString()).SendAsync(ChatHub.ReceiveMessage, message, ct);
            await _hub.Clients.User(message.SenderId.ToString()).SendAsync(ChatHub.ReceiveMessage, message, ct);
        }

        return Json(envelope);
    }

    [HttpPost("conversation/{peerId:guid}/read")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> MarkRead(Guid peerId, CancellationToken ct)
    {
        var envelope = await _chat.MarkReadAsync(peerId, ct);
        return Json(envelope);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> UnreadCount(CancellationToken ct)
    {
        var envelope = await _chat.GetUnreadCountAsync(ct);
        return Json(new { count = envelope.Data });
    }
}

