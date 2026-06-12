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

    [HttpDelete("messages/{messageId:guid}")]
    public async Task<ActionResult<BaseResponse<ChatMessageResponse>>> DeleteMessage(Guid messageId, CancellationToken ct)
    {
        var result = await _chat.DeleteMessageAsync(CurrentUserId, messageId, ct);
        return result is null
            ? NotFound(BaseResponse<ChatMessageResponse>.Failure("Message not found."))
            : Ok(BaseResponse<ChatMessageResponse>.Success(result));
    }

    [HttpPost("messages/{messageId:guid}/forward")]
    public async Task<ActionResult<BaseResponse<ChatMessageResponse>>> Forward(Guid messageId, ForwardChatMessageRequest request, CancellationToken ct)
    {
        request.MessageId = messageId;
        var result = await _chat.ForwardAsync(CurrentUserId, request, ct);
        return result is null
            ? NotFound(BaseResponse<ChatMessageResponse>.Failure("Message not found."))
            : Ok(BaseResponse<ChatMessageResponse>.Success(result));
    }

    [HttpPost("messages/{messageId:guid}/react")]
    public async Task<ActionResult<BaseResponse<ChatMessageResponse>>> React(Guid messageId, ReactChatMessageRequest request, CancellationToken ct)
    {
        var result = await _chat.ToggleReactionAsync(CurrentUserId, messageId, request.Emoji, ct);
        return result is null
            ? NotFound(BaseResponse<ChatMessageResponse>.Failure("Message not found."))
            : Ok(BaseResponse<ChatMessageResponse>.Success(result));
    }

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

    // ----- Groups -----------------------------------------------------------

    [HttpGet("groups")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ChatGroupResponse>>>> GetGroups(CancellationToken ct)
        => Ok(BaseResponse<IReadOnlyList<ChatGroupResponse>>.Success(await _chat.GetGroupsAsync(CurrentUserId, ct)));

    [HttpGet("groups/invites")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ChatGroupInviteResponse>>>> GetGroupInvites(CancellationToken ct)
        => Ok(BaseResponse<IReadOnlyList<ChatGroupInviteResponse>>.Success(await _chat.GetGroupInvitesAsync(CurrentUserId, ct)));

    [HttpPost("groups")]
    public async Task<ActionResult<BaseResponse<ChatGroupResponse>>> CreateGroup(CreateChatGroupRequest request, CancellationToken ct)
        => Ok(BaseResponse<ChatGroupResponse>.Success(await _chat.CreateGroupAsync(CurrentUserId, request, ct)));

    [HttpPost("groups/{groupId:guid}/invite")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<Guid>>>> InviteToGroup(Guid groupId, InviteToGroupRequest request, CancellationToken ct)
        => Ok(BaseResponse<IReadOnlyList<Guid>>.Success(await _chat.InviteToGroupAsync(CurrentUserId, groupId, request, ct)));

    [HttpPost("groups/{groupId:guid}/respond")]
    public async Task<ActionResult<BaseResponse<bool>>> RespondInvite(Guid groupId, RespondGroupInviteRequest request, CancellationToken ct)
        => Ok(BaseResponse<bool>.Success(await _chat.RespondInviteAsync(CurrentUserId, groupId, request.Accept, ct)));

    [HttpGet("groups/{groupId:guid}/members")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ChatGroupMemberResponse>>>> GetGroupMembers(Guid groupId, CancellationToken ct)
        => Ok(BaseResponse<IReadOnlyList<ChatGroupMemberResponse>>.Success(await _chat.GetGroupMembersAsync(CurrentUserId, groupId, ct)));

    [HttpGet("groups/{groupId:guid}/member-ids")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<Guid>>>> GetGroupMemberIds(Guid groupId, CancellationToken ct)
        => Ok(BaseResponse<IReadOnlyList<Guid>>.Success(await _chat.GetGroupMemberIdsAsync(groupId, ct)));

    [HttpGet("groups/{groupId:guid}/conversation")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ChatMessageResponse>>>> GetGroupConversation(Guid groupId, CancellationToken ct)
        => Ok(BaseResponse<IReadOnlyList<ChatMessageResponse>>.Success(await _chat.GetGroupConversationAsync(CurrentUserId, groupId, ct)));
}

