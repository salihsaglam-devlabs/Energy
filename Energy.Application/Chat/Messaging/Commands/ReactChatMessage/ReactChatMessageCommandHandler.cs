using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Chat.Messaging.Commands.ReactChatMessage;

/// <summary><see cref="ReactChatMessageCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class ReactChatMessageCommandHandler
    : IRequestHandler<ReactChatMessageCommand, BaseResponse<ChatMessageResponse>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public ReactChatMessageCommandHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<ChatMessageResponse>> Handle(ReactChatMessageCommand request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var result = await _chat.ToggleReactionAsync(currentUserId, request.MessageId, request.Request.Emoji, ct);
        return result is null
            ? BaseResponse<ChatMessageResponse>.Failure("Message not found.")
            : BaseResponse<ChatMessageResponse>.Success(result);
    }
}
