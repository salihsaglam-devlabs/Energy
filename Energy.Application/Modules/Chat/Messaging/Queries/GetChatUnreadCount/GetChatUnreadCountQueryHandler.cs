using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Queries.GetChatUnreadCount;

/// <summary><see cref="GetChatUnreadCountQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetChatUnreadCountQueryHandler
    : IRequestHandler<GetChatUnreadCountQuery, BaseResponse<int>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public GetChatUnreadCountQueryHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<int>> Handle(GetChatUnreadCountQuery request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var result = await _chat.GetUnreadCountAsync(currentUserId, ct);
        return BaseResponse<int>.Success(result);
    }
}
