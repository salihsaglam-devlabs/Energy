using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Web.Clients.Infrastructure;

namespace Energy.Web.Clients.Chat;

public sealed class ChatApiClient : ApiClientBase, IChatApiClient
{
    public ChatApiClient(HttpClient httpClient) : base(httpClient) { }

    public Task<BaseResponse<IReadOnlyList<ChatContactResponse>>> GetContactsAsync(CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ChatContactResponse>>>(ApiRoutes.Chat.Contacts, ct);

    public Task<BaseResponse<IReadOnlyList<ChatMessageResponse>>> GetConversationAsync(Guid peerId, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ChatMessageResponse>>>(ApiRoutes.Chat.Conversation(peerId), ct);

    public Task<BaseResponse<ChatMessageResponse>> SendAsync(SendChatMessageRequest request, CancellationToken ct = default)
        => PostAsync<SendChatMessageRequest, BaseResponse<ChatMessageResponse>>(ApiRoutes.Chat.Messages, request, ct);

    public Task<BaseResponse<ChatMessageResponse>> DeleteMessageAsync(Guid messageId, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<ChatMessageResponse>>(ApiRoutes.Chat.MessageDelete(messageId), ct);

    public Task<BaseResponse<ChatMessageResponse>> ForwardAsync(Guid messageId, ForwardChatMessageRequest request, CancellationToken ct = default)
        => PostAsync<ForwardChatMessageRequest, BaseResponse<ChatMessageResponse>>(ApiRoutes.Chat.MessageForward(messageId), request, ct);

    public Task<BaseResponse<ChatMessageResponse>> ReactAsync(Guid messageId, ReactChatMessageRequest request, CancellationToken ct = default)
        => PostAsync<ReactChatMessageRequest, BaseResponse<ChatMessageResponse>>(ApiRoutes.Chat.MessageReact(messageId), request, ct);

    public Task<(byte[] Content, string ContentType, int StatusCode)> GetAttachmentAsync(Guid messageId, CancellationToken ct = default)
        => GetRawAsync(ApiRoutes.Chat.MessageAttachment(messageId), ct);

    public Task<(byte[] Content, string ContentType, int StatusCode)> GetUserAvatarAsync(Guid userId, CancellationToken ct = default)
        => GetRawAsync(ApiRoutes.Chat.UserAvatar(userId), ct);

    public Task<BaseResponse<int>> MarkReadAsync(Guid peerId, CancellationToken ct = default)
        => PostAsync<BaseResponse<int>>(ApiRoutes.Chat.MarkRead(peerId), ct);

    public Task<BaseResponse<int>> GetUnreadCountAsync(CancellationToken ct = default)
        => GetAsync<BaseResponse<int>>(ApiRoutes.Chat.UnreadCount, ct);

    // Groups
    public Task<BaseResponse<IReadOnlyList<ChatGroupResponse>>> GetGroupsAsync(CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ChatGroupResponse>>>(ApiRoutes.Chat.Groups, ct);

    public Task<BaseResponse<IReadOnlyList<ChatGroupInviteResponse>>> GetGroupInvitesAsync(CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ChatGroupInviteResponse>>>(ApiRoutes.Chat.GroupInvites, ct);

    public Task<BaseResponse<ChatGroupResponse>> CreateGroupAsync(CreateChatGroupRequest request, CancellationToken ct = default)
        => PostAsync<CreateChatGroupRequest, BaseResponse<ChatGroupResponse>>(ApiRoutes.Chat.Groups, request, ct);

    public Task<BaseResponse<IReadOnlyList<Guid>>> InviteToGroupAsync(Guid groupId, InviteToGroupRequest request, CancellationToken ct = default)
        => PostAsync<InviteToGroupRequest, BaseResponse<IReadOnlyList<Guid>>>(ApiRoutes.Chat.GroupInvite(groupId), request, ct);

    public Task<BaseResponse<bool>> RespondInviteAsync(Guid groupId, RespondGroupInviteRequest request, CancellationToken ct = default)
        => PostAsync<RespondGroupInviteRequest, BaseResponse<bool>>(ApiRoutes.Chat.GroupRespond(groupId), request, ct);

    public Task<BaseResponse<IReadOnlyList<ChatGroupMemberResponse>>> GetGroupMembersAsync(Guid groupId, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ChatGroupMemberResponse>>>(ApiRoutes.Chat.GroupMembers(groupId), ct);

    public Task<BaseResponse<IReadOnlyList<Guid>>> GetGroupMemberIdsAsync(Guid groupId, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<Guid>>>(ApiRoutes.Chat.GroupMemberIds(groupId), ct);

    public Task<BaseResponse<IReadOnlyList<ChatMessageResponse>>> GetGroupConversationAsync(Guid groupId, CancellationToken ct = default)
        => GetAsync<BaseResponse<IReadOnlyList<ChatMessageResponse>>>(ApiRoutes.Chat.GroupConversation(groupId), ct);

    public Task<BaseResponse<bool>> DeleteGroupAsync(Guid groupId, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>(ApiRoutes.Chat.GroupDelete(groupId), ct);

    public Task<BaseResponse<bool>> RemoveMemberAsync(Guid groupId, Guid userId, CancellationToken ct = default)
        => DeleteAsync<BaseResponse<bool>>(ApiRoutes.Chat.GroupRemoveMember(groupId, userId), ct);

    public Task<BaseResponse<bool>> SetGroupAdminAsync(Guid groupId, Guid userId, SetGroupAdminRequest request, CancellationToken ct = default)
        => PostAsync<SetGroupAdminRequest, BaseResponse<bool>>(ApiRoutes.Chat.GroupSetAdmin(groupId, userId), request, ct);
}

