using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Commands.SendChatMessage;

/// <summary><see cref="SendChatMessageCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class SendChatMessageCommandHandler
    : IRequestHandler<SendChatMessageCommand, BaseResponse<ChatMessageResponse>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public SendChatMessageCommandHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<ChatMessageResponse>> Handle(SendChatMessageCommand request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var result = await _chat.SendAsync(currentUserId, request.Request, ct);
        return BaseResponse<ChatMessageResponse>.Success(result);
    }
}
