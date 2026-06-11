using Asp.Versioning;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Shared.Models.V1.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/chat")]
public sealed class ChatController : ControllerBase
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public ChatController(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    private Guid CurrentUserId => _currentUser.UserId ?? Guid.Empty;

    [HttpGet("contacts")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ChatContactResponse>>>> GetContacts(CancellationToken ct)
        => Ok(BaseResponse<IReadOnlyList<ChatContactResponse>>.Success(await _chat.GetContactsAsync(CurrentUserId, ct)));

    [HttpGet("conversation/{peerId:guid}")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ChatMessageResponse>>>> GetConversation(Guid peerId, CancellationToken ct)
        => Ok(BaseResponse<IReadOnlyList<ChatMessageResponse>>.Success(await _chat.GetConversationAsync(CurrentUserId, peerId, ct)));

    [HttpPost("messages")]
    public async Task<ActionResult<BaseResponse<ChatMessageResponse>>> Send(SendChatMessageRequest request, CancellationToken ct)
        => Ok(BaseResponse<ChatMessageResponse>.Success(await _chat.SendAsync(CurrentUserId, request, ct)));

    [HttpGet("messages/{messageId:guid}/attachment")]
    public async Task<IActionResult> GetAttachment(Guid messageId, CancellationToken ct)
    {
        var attachment = await _chat.GetAttachmentAsync(CurrentUserId, messageId, ct);
        return attachment is null
            ? NotFound()
            : File(attachment.Content, attachment.ContentType, attachment.FileName);
    }

    [HttpGet("users/{userId:guid}/avatar")]
    public async Task<IActionResult> GetUserAvatar(Guid userId, CancellationToken ct)
    {
        var avatar = await _chat.GetUserAvatarAsync(userId, ct);
        return avatar is null ? NotFound() : File(avatar.Content, avatar.ContentType);
    }

    [HttpPost("conversation/{peerId:guid}/read")]
    public async Task<ActionResult<BaseResponse<int>>> MarkRead(Guid peerId, CancellationToken ct)
        => Ok(BaseResponse<int>.Success(await _chat.MarkReadAsync(CurrentUserId, peerId, ct)));

    [HttpGet("unread-count")]
    public async Task<ActionResult<BaseResponse<int>>> UnreadCount(CancellationToken ct)
        => Ok(BaseResponse<int>.Success(await _chat.GetUnreadCountAsync(CurrentUserId, ct)));
}

