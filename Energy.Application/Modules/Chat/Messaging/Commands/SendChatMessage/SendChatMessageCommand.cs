using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Chat.Requests;
using Energy.Shared.Models.V1.Chat.Responses;
using MediatR;

namespace Energy.Application.Modules.Chat.Messaging.Commands.SendChatMessage;

/// <summary>SendChatMessage</summary>
public sealed record SendChatMessageCommand(SendChatMessageRequest Request)
    : IRequest<BaseResponse<ChatMessageResponse>>;
