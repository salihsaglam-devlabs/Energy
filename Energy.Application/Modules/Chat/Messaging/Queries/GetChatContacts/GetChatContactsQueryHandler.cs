using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Queries.GetChatContacts;

/// <summary><see cref="GetChatContactsQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetChatContactsQueryHandler
    : IRequestHandler<GetChatContactsQuery, BaseResponse<IReadOnlyList<ChatContactResponse>>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public GetChatContactsQueryHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<IReadOnlyList<ChatContactResponse>>> Handle(GetChatContactsQuery request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var result = await _chat.GetContactsAsync(currentUserId, ct);
        return BaseResponse<IReadOnlyList<ChatContactResponse>>.Success(result);
    }
}
