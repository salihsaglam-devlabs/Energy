using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Commands.RespondChatGroupInvite;

/// <summary>RespondChatGroupInvite</summary>
public sealed record RespondChatGroupInviteCommand(Guid GroupId, RespondGroupInviteRequest Request)
    : IRequest<BaseResponse<bool>>;
