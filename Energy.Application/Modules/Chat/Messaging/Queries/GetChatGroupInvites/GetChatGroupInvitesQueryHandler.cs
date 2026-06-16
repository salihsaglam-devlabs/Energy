using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Queries.GetChatGroupInvites;

/// <summary><see cref="GetChatGroupInvitesQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetChatGroupInvitesQueryHandler
    : IRequestHandler<GetChatGroupInvitesQuery, BaseResponse<IReadOnlyList<ChatGroupInviteResponse>>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public GetChatGroupInvitesQueryHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<IReadOnlyList<ChatGroupInviteResponse>>> Handle(GetChatGroupInvitesQuery request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var result = await _chat.GetGroupInvitesAsync(currentUserId, ct);
        return BaseResponse<IReadOnlyList<ChatGroupInviteResponse>>.Success(result);
    }
}
