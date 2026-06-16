using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Commands.SetChatGroupAdmin;

/// <summary>SetChatGroupAdmin</summary>
public sealed record SetChatGroupAdminCommand(Guid GroupId, Guid UserId, SetGroupAdminRequest Request)
    : IRequest<BaseResponse<bool>>;
