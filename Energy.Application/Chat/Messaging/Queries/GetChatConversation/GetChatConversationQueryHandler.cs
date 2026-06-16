using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Chat.Messaging.Queries.GetChatConversation;

/// <summary><see cref="GetChatConversationQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetChatConversationQueryHandler
    : IRequestHandler<GetChatConversationQuery, BaseResponse<IReadOnlyList<ChatMessageResponse>>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public GetChatConversationQueryHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<IReadOnlyList<ChatMessageResponse>>> Handle(GetChatConversationQuery request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var result = await _chat.GetConversationAsync(currentUserId, request.PeerId, ct);
        return BaseResponse<IReadOnlyList<ChatMessageResponse>>.Success(result);
    }
}
