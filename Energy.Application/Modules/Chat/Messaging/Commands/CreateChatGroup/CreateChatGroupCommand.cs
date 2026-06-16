using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Commands.CreateChatGroup;

/// <summary>CreateChatGroup</summary>
public sealed record CreateChatGroupCommand(CreateChatGroupRequest Request)
    : IRequest<BaseResponse<ChatGroupResponse>>;
