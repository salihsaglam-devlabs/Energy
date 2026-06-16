using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using MediatR;

namespace Energy.Application.Chat.Messaging.Queries.GetChatUserAvatar;

/// <summary><see cref="GetChatUserAvatarQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetChatUserAvatarQueryHandler
    : IRequestHandler<GetChatUserAvatarQuery, ChatAttachmentResponse?>
{
    private readonly IChatService _chat;

    public GetChatUserAvatarQueryHandler(IChatService chat)
    {
        _chat = chat;
    }

    public async Task<ChatAttachmentResponse?> Handle(GetChatUserAvatarQuery request, CancellationToken ct)
    {
        return await _chat.GetUserAvatarAsync(request.UserId, ct);
    }
}
