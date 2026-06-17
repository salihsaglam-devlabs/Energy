using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Chat.Messaging.Commands.SetChatGroupAdmin;

/// <summary><see cref="SetChatGroupAdminCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class SetChatGroupAdminCommandHandler
    : IRequestHandler<SetChatGroupAdminCommand, BaseResponse<bool>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public SetChatGroupAdminCommandHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<bool>> Handle(SetChatGroupAdminCommand request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var ok = await _chat.SetMemberAdminAsync(currentUserId, request.GroupId, request.UserId, request.Request.IsAdmin, ct);
        return ok
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Failure("Member not found or not permitted.");
    }
}
