using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Commands.RemoveChatGroupMember;

/// <summary>RemoveChatGroupMember</summary>
public sealed record RemoveChatGroupMemberCommand(Guid GroupId, Guid UserId)
    : IRequest<BaseResponse<bool>>;
