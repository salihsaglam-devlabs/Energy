using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using Energy.Application.Chat.Services;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Commands.DeleteChatGroup;

/// <summary><see cref="DeleteChatGroupCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class DeleteChatGroupCommandHandler
    : IRequestHandler<DeleteChatGroupCommand, BaseResponse<bool>>
{
    private readonly IChatService _chat;
    private readonly ICurrentUser _currentUser;

    public DeleteChatGroupCommandHandler(IChatService chat, ICurrentUser currentUser)
    {
        _chat = chat;
        _currentUser = currentUser;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteChatGroupCommand request, CancellationToken ct)
    {
        var currentUserId = _currentUser.UserId ?? Guid.Empty;
        var ok = await _chat.DeleteGroupAsync(currentUserId, request.GroupId, ct);
        return ok
            ? BaseResponse<bool>.Success(true)
            : BaseResponse<bool>.Failure("Group not found or not permitted.");
    }
}
