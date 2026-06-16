using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Commands.ReactChatMessage;

/// <summary>ReactChatMessage</summary>
public sealed record ReactChatMessageCommand(Guid MessageId, ReactChatMessageRequest Request)
    : IRequest<BaseResponse<ChatMessageResponse>>;
