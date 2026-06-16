using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Queries.GetChatGroups;

/// <summary><see cref="GetChatGroupsQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetChatGroupsQueryHandler
    : IRequestHandler<GetChatGroupsQuery, BaseResponse<IReadOnlyList<ChatGroupResponse>>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public GetChatGroupsQueryHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<IReadOnlyList<ChatGroupResponse>>> Handle(GetChatGroupsQuery request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var result = await _chat.GetGroupsAsync(currentUserId, ct);
        return BaseResponse<IReadOnlyList<ChatGroupResponse>>.Success(result);
    }
}
