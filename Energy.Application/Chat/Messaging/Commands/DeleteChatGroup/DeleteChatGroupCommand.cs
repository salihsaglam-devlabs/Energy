using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Chat.Messaging.Commands.DeleteChatGroup;

/// <summary>DeleteChatGroup</summary>
public sealed record DeleteChatGroupCommand(Guid GroupId)
    : IRequest<BaseResponse<bool>>;
