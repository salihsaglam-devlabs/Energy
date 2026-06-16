using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Commands.DeleteChatMessage;

/// <summary><see cref="DeleteChatMessageCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class DeleteChatMessageCommandHandler
    : IRequestHandler<DeleteChatMessageCommand, BaseResponse<ChatMessageResponse>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public DeleteChatMessageCommandHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<ChatMessageResponse>> Handle(DeleteChatMessageCommand request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var result = await _chat.DeleteMessageAsync(currentUserId, request.MessageId, ct);
        return result is null
            ? BaseResponse<ChatMessageResponse>.Failure("Message not found.")
            : BaseResponse<ChatMessageResponse>.Success(result);
    }
}
