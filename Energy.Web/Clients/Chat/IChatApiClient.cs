using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Shared.Models.V1.Common.Responses;

namespace Energy.Web.Clients.Chat;

public interface IChatApiClient
{
    Task<BaseResponse<IReadOnlyList<ChatContactResponse>>> GetContactsAsync(CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ChatMessageResponse>>> GetConversationAsync(Guid peerId, CancellationToken ct = default);
    Task<BaseResponse<ChatMessageResponse>> SendAsync(SendChatMessageRequest request, CancellationToken ct = default);
    Task<BaseResponse<ChatMessageResponse>> DeleteMessageAsync(Guid messageId, CancellationToken ct = default);
    Task<BaseResponse<ChatMessageResponse>> ForwardAsync(Guid messageId, ForwardChatMessageRequest request, CancellationToken ct = default);
    Task<BaseResponse<ChatMessageResponse>> ReactAsync(Guid messageId, ReactChatMessageRequest request, CancellationToken ct = default);
    Task<(byte[] Content, string ContentType, int StatusCode)> GetAttachmentAsync(Guid messageId, CancellationToken ct = default);
    Task<(byte[] Content, string ContentType, int StatusCode)> GetUserAvatarAsync(Guid userId, CancellationToken ct = default);
    Task<BaseResponse<int>> MarkReadAsync(Guid peerId, CancellationToken ct = default);
    Task<BaseResponse<int>> GetUnreadCountAsync(CancellationToken ct = default);

    // Groups
    Task<BaseResponse<IReadOnlyList<ChatGroupResponse>>> GetGroupsAsync(CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ChatGroupInviteResponse>>> GetGroupInvitesAsync(CancellationToken ct = default);
    Task<BaseResponse<ChatGroupResponse>> CreateGroupAsync(CreateChatGroupRequest request, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<Guid>>> InviteToGroupAsync(Guid groupId, InviteToGroupRequest request, CancellationToken ct = default);
    Task<BaseResponse<bool>> RespondInviteAsync(Guid groupId, RespondGroupInviteRequest request, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ChatGroupMemberResponse>>> GetGroupMembersAsync(Guid groupId, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<Guid>>> GetGroupMemberIdsAsync(Guid groupId, CancellationToken ct = default);
    Task<BaseResponse<IReadOnlyList<ChatMessageResponse>>> GetGroupConversationAsync(Guid groupId, CancellationToken ct = default);
}

