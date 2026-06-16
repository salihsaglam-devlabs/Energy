using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Chat.Messaging.Commands.CreateChatGroup;

/// <summary><see cref="CreateChatGroupCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class CreateChatGroupCommandHandler
    : IRequestHandler<CreateChatGroupCommand, BaseResponse<ChatGroupResponse>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public CreateChatGroupCommandHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<ChatGroupResponse>> Handle(CreateChatGroupCommand request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var result = await _chat.CreateGroupAsync(currentUserId, request.Request, ct);
        return BaseResponse<ChatGroupResponse>.Success(result);
    }
}
