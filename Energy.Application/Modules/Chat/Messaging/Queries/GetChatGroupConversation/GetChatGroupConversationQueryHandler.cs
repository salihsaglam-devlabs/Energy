using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Queries.GetChatGroupConversation;

/// <summary><see cref="GetChatGroupConversationQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetChatGroupConversationQueryHandler
    : IRequestHandler<GetChatGroupConversationQuery, BaseResponse<IReadOnlyList<ChatMessageResponse>>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public GetChatGroupConversationQueryHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<IReadOnlyList<ChatMessageResponse>>> Handle(GetChatGroupConversationQuery request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var result = await _chat.GetGroupConversationAsync(currentUserId, request.GroupId, ct);
        return BaseResponse<IReadOnlyList<ChatMessageResponse>>.Success(result);
    }
}
