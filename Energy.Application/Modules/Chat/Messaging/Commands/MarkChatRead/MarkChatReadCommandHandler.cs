using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Commands.MarkChatRead;

/// <summary><see cref="MarkChatReadCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class MarkChatReadCommandHandler
    : IRequestHandler<MarkChatReadCommand, BaseResponse<int>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public MarkChatReadCommandHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<int>> Handle(MarkChatReadCommand request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var result = await _chat.MarkReadAsync(currentUserId, request.PeerId, ct);
        return BaseResponse<int>.Success(result);
    }
}
