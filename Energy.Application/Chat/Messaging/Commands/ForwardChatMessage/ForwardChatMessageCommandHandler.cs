using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Chat.Messaging.Commands.ForwardChatMessage;

/// <summary><see cref="ForwardChatMessageCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class ForwardChatMessageCommandHandler
    : IRequestHandler<ForwardChatMessageCommand, BaseResponse<ChatMessageResponse>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public ForwardChatMessageCommandHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<ChatMessageResponse>> Handle(ForwardChatMessageCommand request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        request.Request.MessageId = request.MessageId;
        var result = await _chat.ForwardAsync(currentUserId, request.Request, ct);
        return result is null
            ? BaseResponse<ChatMessageResponse>.Failure("Message not found.")
            : BaseResponse<ChatMessageResponse>.Success(result);
    }
}
