using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Commands.RespondChatGroupInvite;

/// <summary><see cref="RespondChatGroupInviteCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class RespondChatGroupInviteCommandHandler
    : IRequestHandler<RespondChatGroupInviteCommand, BaseResponse<bool>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public RespondChatGroupInviteCommandHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<bool>> Handle(RespondChatGroupInviteCommand request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var result = await _chat.RespondInviteAsync(currentUserId, request.GroupId, request.Request.Accept, ct);
        return BaseResponse<bool>.Success(result);
    }
}
