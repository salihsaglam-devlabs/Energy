using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Chat.Messaging.Commands.ForwardChatMessage;

/// <summary>ForwardChatMessage</summary>
public sealed record ForwardChatMessageCommand(Guid MessageId, ForwardChatMessageRequest Request)
    : IRequest<BaseResponse<ChatMessageResponse>>;
