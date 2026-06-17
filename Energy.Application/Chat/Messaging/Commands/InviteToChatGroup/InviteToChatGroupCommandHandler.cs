using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Chat.Messaging.Commands.InviteToChatGroup;

/// <summary><see cref="InviteToChatGroupCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class InviteToChatGroupCommandHandler
    : IRequestHandler<InviteToChatGroupCommand, BaseResponse<IReadOnlyList<Guid>>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public InviteToChatGroupCommandHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<IReadOnlyList<Guid>>> Handle(InviteToChatGroupCommand request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var result = await _chat.InviteToGroupAsync(currentUserId, request.GroupId, request.Request, ct);
        return BaseResponse<IReadOnlyList<Guid>>.Success(result);
    }
}
