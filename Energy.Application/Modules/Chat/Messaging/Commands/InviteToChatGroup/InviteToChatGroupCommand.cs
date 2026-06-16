using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Commands.InviteToChatGroup;

/// <summary>InviteToChatGroup</summary>
public sealed record InviteToChatGroupCommand(Guid GroupId, InviteToGroupRequest Request)
    : IRequest<BaseResponse<IReadOnlyList<Guid>>>;
