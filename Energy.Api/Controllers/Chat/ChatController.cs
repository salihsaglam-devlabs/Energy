using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Messaging.Commands.CreateChatGroup;
using Energy.Application.Chat.Messaging.Commands.DeleteChatGroup;
using Energy.Application.Chat.Messaging.Commands.DeleteChatMessage;
using Energy.Application.Chat.Messaging.Commands.ForwardChatMessage;
using Energy.Application.Chat.Messaging.Commands.InviteToChatGroup;
using Energy.Application.Chat.Messaging.Commands.MarkChatRead;
using Energy.Application.Chat.Messaging.Commands.ReactChatMessage;
using Energy.Application.Chat.Messaging.Commands.RemoveChatGroupMember;
using Energy.Application.Chat.Messaging.Commands.RespondChatGroupInvite;
using Energy.Application.Chat.Messaging.Commands.SendChatMessage;
using Energy.Application.Chat.Messaging.Commands.SetChatGroupAdmin;
using Energy.Application.Chat.Messaging.Queries.GetChatAttachment;
using Energy.Application.Chat.Messaging.Queries.GetChatContacts;
using Energy.Application.Chat.Messaging.Queries.GetChatConversation;
using Energy.Application.Chat.Messaging.Queries.GetChatGroupConversation;
using Energy.Application.Chat.Messaging.Queries.GetChatGroupInvites;
using Energy.Application.Chat.Messaging.Queries.GetChatGroupMemberIds;
using Energy.Application.Chat.Messaging.Queries.GetChatGroupMembers;
using Energy.Application.Chat.Messaging.Queries.GetChatGroups;
using Energy.Application.Chat.Messaging.Queries.GetChatUnreadCount;
using Energy.Application.Chat.Messaging.Queries.GetChatUserAvatar;

namespace Energy.Api.Controllers.Chat;

/// <summary>Sohbet uç noktaları (kişisel + grup, mesaj, reaksiyon, ek).</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/chat")]
public sealed class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("contacts")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ChatContactResponse>>>> GetContacts(CancellationToken ct)
        => Ok(await _mediator.Send(new GetChatContactsQuery(), ct));

    [HttpGet("conversation/{peerId:guid}")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ChatMessageResponse>>>> GetConversation(Guid peerId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetChatConversationQuery(peerId), ct));

    [HttpPost("messages")]
    public async Task<ActionResult<BaseResponse<ChatMessageResponse>>> Send(SendChatMessageRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new SendChatMessageCommand(request), ct));

    [HttpDelete("messages/{messageId:guid}")]
    public async Task<ActionResult<BaseResponse<ChatMessageResponse>>> DeleteMessage(Guid messageId, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteChatMessageCommand(messageId), ct));

    [HttpPost("messages/{messageId:guid}/forward")]
    public async Task<ActionResult<BaseResponse<ChatMessageResponse>>> Forward(Guid messageId, ForwardChatMessageRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new ForwardChatMessageCommand(messageId, request), ct));

    [HttpPost("messages/{messageId:guid}/react")]
    public async Task<ActionResult<BaseResponse<ChatMessageResponse>>> React(Guid messageId, ReactChatMessageRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new ReactChatMessageCommand(messageId, request), ct));

    [HttpGet("messages/{messageId:guid}/attachment")]
    public async Task<IActionResult> GetAttachment(Guid messageId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetChatAttachmentQuery(messageId), ct);
        return result is null ? NotFound() : File(result.Content, result.ContentType, result.FileName);
    }

    [HttpGet("users/{userId:guid}/avatar")]
    public async Task<IActionResult> GetUserAvatar(Guid userId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetChatUserAvatarQuery(userId), ct);
        return result is null ? NotFound() : File(result.Content, result.ContentType);
    }

    [HttpPost("conversation/{peerId:guid}/read")]
    public async Task<ActionResult<BaseResponse<int>>> MarkRead(Guid peerId, CancellationToken ct)
        => Ok(await _mediator.Send(new MarkChatReadCommand(peerId), ct));

    [HttpGet("unread-count")]
    public async Task<ActionResult<BaseResponse<int>>> UnreadCount(CancellationToken ct)
        => Ok(await _mediator.Send(new GetChatUnreadCountQuery(), ct));

    [HttpGet("groups")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ChatGroupResponse>>>> GetGroups(CancellationToken ct)
        => Ok(await _mediator.Send(new GetChatGroupsQuery(), ct));

    [HttpGet("groups/invites")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ChatGroupInviteResponse>>>> GetGroupInvites(CancellationToken ct)
        => Ok(await _mediator.Send(new GetChatGroupInvitesQuery(), ct));

    [HttpPost("groups")]
    public async Task<ActionResult<BaseResponse<ChatGroupResponse>>> CreateGroup(CreateChatGroupRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateChatGroupCommand(request), ct));

    [HttpPost("groups/{groupId:guid}/invite")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<Guid>>>> InviteToGroup(Guid groupId, InviteToGroupRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new InviteToChatGroupCommand(groupId, request), ct));

    [HttpPost("groups/{groupId:guid}/respond")]
    public async Task<ActionResult<BaseResponse<bool>>> RespondInvite(Guid groupId, RespondGroupInviteRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new RespondChatGroupInviteCommand(groupId, request), ct));

    [HttpGet("groups/{groupId:guid}/members")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ChatGroupMemberResponse>>>> GetGroupMembers(Guid groupId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetChatGroupMembersQuery(groupId), ct));

    [HttpGet("groups/{groupId:guid}/member-ids")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<Guid>>>> GetGroupMemberIds(Guid groupId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetChatGroupMemberIdsQuery(groupId), ct));

    [HttpGet("groups/{groupId:guid}/conversation")]
    public async Task<ActionResult<BaseResponse<IReadOnlyList<ChatMessageResponse>>>> GetGroupConversation(Guid groupId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetChatGroupConversationQuery(groupId), ct));

    [HttpDelete("groups/{groupId:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteGroup(Guid groupId, CancellationToken ct)
        => Ok(await _mediator.Send(new DeleteChatGroupCommand(groupId), ct));

    [HttpDelete("groups/{groupId:guid}/members/{userId:guid}")]
    public async Task<ActionResult<BaseResponse<bool>>> RemoveMember(Guid groupId, Guid userId, CancellationToken ct)
        => Ok(await _mediator.Send(new RemoveChatGroupMemberCommand(groupId, userId), ct));

    [HttpPost("groups/{groupId:guid}/members/{userId:guid}/admin")]
    public async Task<ActionResult<BaseResponse<bool>>> SetGroupAdmin(Guid groupId, Guid userId, SetGroupAdminRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new SetChatGroupAdminCommand(groupId, userId, request), ct));
}
