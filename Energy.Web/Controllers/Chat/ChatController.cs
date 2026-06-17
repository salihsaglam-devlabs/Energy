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

namespace Energy.Web.Controllers.Chat;

/// <summary>
/// Sohbet sayfası bir DevExtreme dxChat kabuğudur. Veri API'ye vekillenir (her mesajı
/// kalıcı hale getiren) ve gerçek zamanlı teslimat üstüne SignalR <see cref="ChatHub"/>
/// ile eklenir: bir mesaj saklandıktan sonra alıcıya (ve gönderenin diğer sekmelerine)
/// gönderilir; böylece zil/rozet canlı güncellenir.
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

        // Oturum açmış kullanıcının profil resmi olup olmadığı; böylece kendi mesajları
        // da sohbet penceresinde bir avatarla görüntülenir. Profil/kullanıcı yönetimi
        // yetkisi gerekmemesi için sohbet kapsamlı avatar uç noktasını kullanır.
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

    /// <summary>Sohbetin avatar olarak gösterebilmesi için bir kullanıcının profil resmini akıtır.</summary>
    [HttpGet("avatar/{userId:guid}")]
    public async Task<IActionResult> Avatar(Guid userId, CancellationToken ct)
    {
        // Sohbet kapsamlı API uç noktası (ChatUse) üzerinden sunulur; böylece
        // kullanıcı yönetimi/profil yetkisi olmayan kullanıcılar için bile karşı
        // tarafların avatarları yüklenir.
        var (content, contentType, status) = await _chat.GetUserAvatarAsync(userId, ct);
        if (status != 200 || content.Length == 0)
        {
            return NotFound();
        }
        return File(content, string.IsNullOrWhiteSpace(contentType) ? "image/png" : contentType);
    }

    /// <summary>Bir sohbet mesajı içinde paylaşılan dosyayı akıtır (yalnızca katılımcılar; API tarafından uygulanır).</summary>
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
                // Kabul edilmiş her grup üyesine dağıt (gönderenin sekmeleri dahil).
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
                // Alıcıya ilet (canlı ekleme + zil) ve her yüzeyin senkronize kalması
                // için gönderenin diğer açık sekmelerine de yansıt.
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

        // Okundu bilgisi: karşı tarafa (asıl gönderene) mesajlarını okuduğumuzu
        // bildir; böylece tik işaretleri "okundu"ya döner.
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

    // (Yeni oluşturulan) bir mesajı alıcısına/grubuna + gönderenin sekmelerine iletir.
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

    // Bir mesajın her katılımcısına rastgele bir olay/yük gönderir.
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

    [HttpDelete("groups/{groupId:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> DeleteGroup(Guid groupId, CancellationToken ct)
    {
        // Silmeden ÖNCE üye listesini yakala; böylece yine herkese bildirim gönderebiliriz.
        var memberEnvelope = await _chat.GetGroupMemberIdsAsync(groupId, ct);
        var memberIds = (memberEnvelope.Data ?? Array.Empty<Guid>()).Select(id => id.ToString()).ToArray();

        var envelope = await _chat.DeleteGroupAsync(groupId, ct);
        if (envelope.IsSuccess && envelope.Data && memberIds.Length > 0)
        {
            await _hub.Clients.Users(memberIds).SendAsync(ChatHub.GroupDeleted, new { groupId = groupId.ToString() }, ct);
        }
        return Json(envelope);
    }

    [HttpDelete("groups/{groupId:guid}/members/{userId:guid}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> RemoveMember(Guid groupId, Guid userId, CancellationToken ct)
    {
        // Çıkarmadan önce üye listesinin (çıkmak üzere olan kullanıcı dahil) anlık görüntüsünü al.
        var beforeEnvelope = await _chat.GetGroupMemberIdsAsync(groupId, ct);
        var beforeIds = (beforeEnvelope.Data ?? Array.Empty<Guid>()).Select(id => id.ToString()).ToHashSet();
        beforeIds.Add(userId.ToString());

        var envelope = await _chat.RemoveMemberAsync(groupId, userId, ct);
        if (envelope.IsSuccess && envelope.Data && beforeIds.Count > 0)
        {
            await _hub.Clients.Users(beforeIds.ToArray()).SendAsync(ChatHub.GroupChanged, new { groupId = groupId.ToString() }, ct);
        }
        return Json(envelope);
    }

    public sealed class SetAdminInput
    {
        public bool IsAdmin { get; set; }
    }

    [HttpPost("groups/{groupId:guid}/members/{userId:guid}/admin")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> SetGroupAdmin(Guid groupId, Guid userId, [FromBody] SetAdminInput input, CancellationToken ct)
    {
        var envelope = await _chat.SetGroupAdminAsync(groupId, userId,
            new Shared.Models.V1.Chat.Requests.SetGroupAdminRequest { IsAdmin = input.IsAdmin }, ct);

        if (envelope.IsSuccess && envelope.Data)
        {
            var memberEnvelope = await _chat.GetGroupMemberIdsAsync(groupId, ct);
            var memberIds = (memberEnvelope.Data ?? Array.Empty<Guid>()).Select(id => id.ToString()).ToArray();
            if (memberIds.Length > 0)
            {
                await _hub.Clients.Users(memberIds).SendAsync(ChatHub.GroupChanged, new { groupId = groupId.ToString() }, ct);
            }
        }
        return Json(envelope);
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
            // Mevcut üyelere listenin değiştiğini bildir (üyeleri/grupları yenile).
            var memberEnvelope = await _chat.GetGroupMemberIdsAsync(groupId, ct);
            var memberIds = (memberEnvelope.Data ?? Array.Empty<Guid>()).Select(id => id.ToString()).ToArray();
            if (memberIds.Length > 0)
            {
                await _hub.Clients.Users(memberIds).SendAsync(ChatHub.GroupChanged, new { groupId = groupId.ToString() }, ct);
            }
        }

        return Json(envelope);
    }

    // Her davet edilen kişinin açık sekmelerine bir "davet edildiniz" olayı gönderir.
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

