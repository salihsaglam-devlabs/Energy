using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Chat.Messaging.Commands.DeleteChatMessage;

/// <summary>DeleteChatMessage</summary>
public sealed record DeleteChatMessageCommand(Guid MessageId)
    : IRequest<BaseResponse<ChatMessageResponse>>;
